using System.Net;
using System.Text;
using ClinicService.API.ViewModels;
using ClinicService.DAL.Entities;
using ClinicService.Test.TestEntities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ClinicService.Test.IntergationTests;
public class DoctorIntegrationTests : IntegrationTests
{
    [Fact]
    public async Task Create_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7105/Doctor");
        request.Content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

        //Act
        var actualResult = await Client.SendAsync(request);

        var responseResult = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult.Content.ReadAsStringAsync().Result);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
        Assert.Equivalent(responseResult, viewModel);
    }

    [Fact]
    public async Task Get_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7105/Doctor");
        var r = AddContent(viewModel, request);
        var actualResult = await Client.SendAsync(r);
        var responseResult = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult.Content.ReadAsStringAsync().Result);

        //Act
        using var request2 = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7105/Doctor?id={responseResult.Id}");
        var actualResult2 = await Client.SendAsync(request2);
        var responseResult2 = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult2.Content.ReadAsStringAsync().Result);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult2.StatusCode);
        Assert.Equivalent(responseResult2, viewModel);
    }

    [Fact]
    public async Task Put_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;

        var changeViewModel = TestDoctorViewModel.UpdatedDoctorViewModel;

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7105/Doctor");
        var r = AddContent(viewModel, request);
        var actualResult = await Client.SendAsync(r);
        var responseResult = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult.Content.ReadAsStringAsync().Result);

        //Act
        using var request2 = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7105/Doctor");
        request2.Content = new StringContent(JsonConvert.SerializeObject(changeViewModel), Encoding.UTF8, "application/json");
        var actualResult2 = await Client.SendAsync(request2);
        var responseResult2 = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult2.Content.ReadAsStringAsync().Result);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult2.StatusCode);
        Assert.Equivalent(responseResult2, changeViewModel);
    }

    [Fact]
    public async Task Delete_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        var viewModel = TestDoctorViewModel.NewDoctorViewModel;
        var entity = TestDoctorEntity.Doctor;

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7105/Doctor");
        request.Content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");
        var actualResult = await Client.SendAsync(request);
        var responseResult = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult.Content.ReadAsStringAsync().Result);

        //Act
        using var request2 = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7105/Doctor?id={responseResult.Id}");
        var actualResult2 = await Client.SendAsync(request2);

        //Assert
        Assert.False(Context.Set<DoctorEntity>().Contains(entity));
        Assert.Equal(HttpStatusCode.OK, actualResult2.StatusCode);
    }
}
