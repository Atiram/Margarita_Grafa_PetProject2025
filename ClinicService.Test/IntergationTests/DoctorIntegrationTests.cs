using System.Net;
using ClinicService.API.ViewModels;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Enums;
using ClinicService.DAL.Utilities.Pagination;
using ClinicService.Test.TestEntities;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Test.IntergationTests;
public class DoctorIntegrationTests : IntegrationTests
{
    private const string BaseUrl = "https://localhost:7105/Doctor";
    private const string GetAllUrl = "/GetAll";

    [Fact]
    public async Task Create_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;

        viewModel.Id = Guid.NewGuid();
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl);
        var actualRequest = AddContent(viewModel, request);

        //Act
        var actualResult = await Client.SendAsync(actualRequest);
        var responseResult = GetResponseResult<DoctorViewModel>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(responseResult, viewModel);
    }

    [Fact]
    public async Task Get_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;
        viewModel.Id = Guid.NewGuid();

        var postResponse = await SendPostRequest(viewModel);
        var postResponseResult = GetResponseResult<DoctorViewModel>(postResponse);

        Assert.NotNull(postResponseResult);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{postResponseResult.Id}");
        var actualResult = await Client.SendAsync(request);
        var responseResult = GetResponseResult<DoctorViewModel>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(responseResult, viewModel);
    }

    [Fact]
    public async Task GetAll_ValidViewModel_ReturnsViewModels()
    {
        //Arrange
        var searchPrefix = Guid.NewGuid().ToString();
        var doctorViewModels = CreateDoctorList(searchPrefix);
        foreach (var viewModel in doctorViewModels)
        {
            await SendPostRequest(viewModel);
        }
        var getAllDoctorsParams = CreateGetAllDoctorsParams(searchPrefix);
        var expectedPagedResult = CreatePagedResult(getAllDoctorsParams, doctorViewModels);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + GetAllUrl);
        var actualRequest = AddContent(getAllDoctorsParams, request);

        var actualResult = await Client.SendAsync(actualRequest);
        var responseResult = GetResponseResult<PagedResult<DoctorViewModel>>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(expectedPagedResult, responseResult);
    }

    [Fact]
    public async Task GetSorted_ValidViewModel_ReturnsViewModels()
    {
        //Arrange
        var searchPrefix = Guid.NewGuid().ToString();
        var doctorViewModels = CreateDoctorList(searchPrefix);
        foreach (var viewModel in doctorViewModels)
        {
            viewModel.LastName = new Random().Next(10) + viewModel.LastName;
            await SendPostRequest(viewModel);
        }
        var getAllDoctorsParams = CreateGetAllDoctorsParams(searchPrefix);
        getAllDoctorsParams.SortParameter = DoctorSortingParams.LastName;

        var expectedPagedResult = CreatePagedResult(getAllDoctorsParams, doctorViewModels);
        expectedPagedResult.Results = doctorViewModels
                .OrderBy(x => x.LastName)
                .Skip((getAllDoctorsParams.PageNumber - 1) * getAllDoctorsParams.PageSize)
                .Take(getAllDoctorsParams.PageSize)
                .ToList();

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + GetAllUrl);
        var actualRequest = AddContent(getAllDoctorsParams, request);

        var actualResult = await Client.SendAsync(actualRequest);
        var responseResult = GetResponseResult<PagedResult<DoctorViewModel>>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(expectedPagedResult, responseResult);
    }

    [Fact]
    public async Task GetSearched_ValidViewModel_ReturnsViewModels()
    {
        //Arrange
        var searchPrefix = Guid.NewGuid().ToString();
        var doctorViewModels = CreateDoctorList(searchPrefix);
        foreach (var viewModel in doctorViewModels)
        {
            viewModel.LastName = new Random().Next(10) + viewModel.LastName;
            await SendPostRequest(viewModel);
        }
        var getAllDoctorsParams = CreateGetAllDoctorsParams(searchPrefix);
        string searchValue = doctorViewModels[0].LastName;
        getAllDoctorsParams.SearchValue = searchValue;

        var expectedPagedResult = CreatePagedResult(getAllDoctorsParams, doctorViewModels);
        expectedPagedResult.Results = doctorViewModels
                .Where(doctor => doctor.LastName.Contains(searchValue))
                .Skip((getAllDoctorsParams.PageNumber - 1) * getAllDoctorsParams.PageSize)
                .Take(getAllDoctorsParams.PageSize)
                .ToList();
        expectedPagedResult.TotalCount = doctorViewModels
                .Where(doctor => doctor.LastName.Contains(searchValue))
                .Count();
        //Act
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + GetAllUrl);
        var actualRequest = AddContent(getAllDoctorsParams, request);

        var actualResult = await Client.SendAsync(actualRequest);
        var responseResult = GetResponseResult<PagedResult<DoctorViewModel>>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(expectedPagedResult, responseResult);
    }

    [Fact]
    public async Task Put_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;
        var updatedViewModel = TestDoctorViewModel.UpdatedDoctorViewModel;
        viewModel.Id = Guid.NewGuid();
        updatedViewModel.Id = viewModel.Id;

        var postResponse = await SendPostRequest(viewModel);
        var postResponseResult = GetResponseResult<DoctorViewModel>(postResponse);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Put, BaseUrl);
        var actualRequest = AddContent(updatedViewModel, request);

        var actualResult = await Client.SendAsync(actualRequest);
        var responseResult = GetResponseResult<DoctorViewModel>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(responseResult, updatedViewModel);
    }

    [Fact]
    public async Task Delete_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;
        var entity = TestDoctorEntity.NewDoctorEntity;

        var postResponse = await SendPostRequest(viewModel);
        var postResponseResult = GetResponseResult<DoctorViewModel>(postResponse);

        Assert.NotNull(postResponseResult);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}?id={postResponseResult.Id}");
        var actualResult = await Client.SendAsync(request);

        //Assert
        Assert.False(Context.Set<DoctorEntity>().Contains(entity));
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
    }
}
