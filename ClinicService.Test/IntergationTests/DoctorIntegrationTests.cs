using System.Net;
using ClinicService.DAL.Entities;
using ClinicService.Test.TestEntities;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Test.IntergationTests;
public class DoctorIntegrationTests : IntegrationTests
{
    private const string BasetUrl = "https://localhost:7105/Doctor";

    [Fact]
    public async Task Create_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;

        viewModel.Id = Guid.NewGuid();
        using var request = new HttpRequestMessage(HttpMethod.Post, BasetUrl);
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
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BasetUrl}?id={postResponseResult.Id}");
        var actualResult = await Client.SendAsync(request);
        var responseResult = GetResponseResult(actualResult);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(responseResult, viewModel);
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
        using var request = new HttpRequestMessage(HttpMethod.Put, BasetUrl);
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
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{BasetUrl}?id={postResponseResult.Id}");
        var actualResult = await Client.SendAsync(request);

        //Assert
        Assert.False(Context.Set<DoctorEntity>().Contains(entity));
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
    }
}
