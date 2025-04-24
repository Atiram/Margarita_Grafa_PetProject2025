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

    [Fact]
    public async Task Create_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var createDoctorRequest = TestDoctorRequest.NewCreateDoctorRequest;
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl);
        var actualRequest = AddContent(createDoctorRequest, request);

        //Act
        var actualResult = await Client.SendAsync(actualRequest);
        var responseResult = GetResponseResult<DoctorViewModel>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.NotNull(responseResult);
        Assert.Equivalent(createDoctorRequest.LastName, responseResult.LastName);
        Assert.Equivalent(createDoctorRequest.FirstName, responseResult.FirstName);
    }

    [Fact]
    public async Task Get_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var createDoctorRequest = TestDoctorRequest.NewCreateDoctorRequest;
        var postResponse = await SendPostRequest(createDoctorRequest);
        var postResponseResult = GetResponseResult<DoctorViewModel>(postResponse);
        Assert.NotNull(postResponseResult);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{postResponseResult.Id}");
        var actualResult = await Client.SendAsync(request);
        var responseResult = GetResponseResult<DoctorViewModel>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.NotNull(responseResult);
        Assert.Equivalent(createDoctorRequest.LastName, responseResult.LastName);
        Assert.Equivalent(createDoctorRequest.FirstName, responseResult.FirstName);
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
        string actualUrl = CreateActualUrl(getAllDoctorsParams);
        using var request = new HttpRequestMessage(HttpMethod.Get, actualUrl);

        var actualResult = await Client.SendAsync(request);
        var responseResult = GetResponseResult<PagedResult<DoctorViewModel>>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(expectedPagedResult.TotalCount, responseResult.TotalCount);
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
        string actualUrl = CreateActualUrl(getAllDoctorsParams);
        using var request = new HttpRequestMessage(HttpMethod.Get, actualUrl);

        var actualResult = await Client.SendAsync(request);
        var responseResult = GetResponseResult<PagedResult<DoctorViewModel>>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(expectedPagedResult.TotalCount, responseResult.TotalCount);
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
        string actualUrl = CreateActualUrl(getAllDoctorsParams);
        using var request = new HttpRequestMessage(HttpMethod.Get, actualUrl);

        var actualResult = await Client.SendAsync(request);
        var responseResult = GetResponseResult<PagedResult<DoctorViewModel>>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(expectedPagedResult.TotalCount, responseResult.TotalCount);
    }

    [Fact]
    public async Task Put_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var createDoctorRequest = TestDoctorRequest.NewCreateDoctorRequest;
        var updateDoctorRequest = TestDoctorRequest.UpdatedDoctorRequest();

        var postResponse = await SendPostRequest(createDoctorRequest);
        var postResponseResult = GetResponseResult<DoctorViewModel>(postResponse);
        Assert.NotNull(postResponseResult);
        updateDoctorRequest.Id = postResponseResult.Id;
        //Act
        using var request = new HttpRequestMessage(HttpMethod.Put, BaseUrl);
        var actualRequest = AddContent(updateDoctorRequest, request);

        var actualResult = await Client.SendAsync(actualRequest);
        var responseResult = GetResponseResult<DoctorViewModel>(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.NotNull(responseResult);
        Assert.Equivalent(updateDoctorRequest.LastName, responseResult.LastName);
        Assert.Equivalent(updateDoctorRequest.FirstName, responseResult.FirstName);
    }

    [Fact]
    public async Task Delete_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var entity = TestDoctorEntity.NewDoctorEntity;
        var createDoctorRequest = TestDoctorRequest.NewCreateDoctorRequest;

        var postResponse = await SendPostRequest(createDoctorRequest);
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
