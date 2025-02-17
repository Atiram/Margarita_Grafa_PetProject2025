using System.Net;
using ClinicService.DAL.Entities;
using ClinicService.Test.TestEntities;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Test.IntergationTests;
public class DoctorIntegrationTests : IntegrationTests
{
    [Fact]
    public async Task Create_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7105/Doctor");
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


        var postResponse = await SendPostRequest(viewModel);
        var postResponseResult = GetResponseResult(postResponse);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7105/Doctor?id={postResponseResult.Id}");
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

        var postResponse = await SendPostRequest(viewModel);
        var postResponseResult = GetResponseResult(postResponse);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7105/Doctor");
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
        var entity = TestDoctorEntity.Doctor;

        var postResponse = await SendPostRequest(viewModel);
        var postResponseResult = GetResponseResult(postResponse);

        //Act
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7105/Doctor?id={postResponseResult.Id}");
        var actualResult = await Client.SendAsync(request);

        //Assert
        Assert.False(Context.Set<DoctorEntity>().Contains(entity));
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
    }
}
