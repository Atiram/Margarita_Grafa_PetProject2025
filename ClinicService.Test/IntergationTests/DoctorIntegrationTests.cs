using System.Net;
using System.Text;
using ClinicService.API.ViewModels;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Utilities.Pagination;
using ClinicService.Test.TestEntities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ClinicService.Test.IntergationTests;
public class DoctorIntegrationTests : IntegrationTests
{
    private const string BaseUrl = "https://localhost:7105/Doctor";

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
        var responseResult = GetResponseResult(actualResult);

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
        var postResponseResult = GetResponseResult(postResponse);

        Assert.NotNull(postResponseResult);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{postResponseResult.Id}");
        var actualResult = await Client.SendAsync(request);
        var responseResult = GetResponseResult(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(responseResult, viewModel);
    }

    [Fact]
    public async Task GetAll_ValidViewModel_ReturnsViewModels()
    {
        //Arrange
        var doctorViewModels = new List<DoctorViewModel>();

        for (int i = 0; i < 5; i++)
        {
            var viewModel = TestDoctorViewModel.NewDoctorViewModel;
            viewModel.Id = Guid.NewGuid();
            doctorViewModels.Add(viewModel);
            var postResponse = await SendPostRequest(viewModel);
        }
        GetAllDoctorsParams getAllDoctorsParams = new GetAllDoctorsParams()
        {
            IsDescending = false,
            PageNumber = 1,
            PageSize = 10,
            SortParameter = null,
            SearchValue = null,
        };
        PagedResult<DoctorViewModel> expectedPagedResult = new PagedResult<DoctorViewModel>()
        {
            PageSize = getAllDoctorsParams.PageSize,
            TotalCount = doctorViewModels.Count,
            TotalPages = (int)Math.Ceiling((double)getAllDoctorsParams.PageNumber / getAllDoctorsParams.PageSize),
            Results = doctorViewModels
        };
        //Act
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/GetAll");
        request.Content = new StringContent(JsonConvert.SerializeObject(getAllDoctorsParams), Encoding.UTF8, "application/json");

        var actualResult = await Client.SendAsync(request);
        var responseResult = JsonConvert.DeserializeObject<PagedResult<DoctorViewModel>>(actualResult.Content.ReadAsStringAsync().Result);

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
        var postResponseResult = GetResponseResult(postResponse);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Put, BaseUrl);
        var actualRequest = AddContent(updatedViewModel, request);
        var actualResult = await Client.SendAsync(actualRequest);
        var responseResult = GetResponseResult(actualResult);

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
        var postResponseResult = GetResponseResult(postResponse);

        Assert.NotNull(postResponseResult);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}?id={postResponseResult.Id}");
        var actualResult = await Client.SendAsync(request);

        //Assert
        Assert.False(Context.Set<DoctorEntity>().Contains(entity));
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
    }
}
