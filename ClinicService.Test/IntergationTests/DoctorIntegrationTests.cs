using ClinicService.API.ViewModels;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Entities.Enums;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ClinicService.Test.IntergationTests;
public class DoctorIntegrationTests : IntegrationTests
{
    [Fact]
    public async Task Create_ValidViewModel_ReturnsViewModel()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var viewModel = new DoctorViewModel
        {
            Id = id,
            FirstName = "Test DoctorName",
            LastName = "Test LastName",
            MiddleName = "Test MiddleName",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Email = "test@email",
            Specialization = "TestSpec",
            Office = "TestOffice",
            CareerStartYear = 2000,
            Status = DoctorStatus.AtWork
        };

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
        Guid id = Guid.NewGuid();
        var viewModel = new DoctorViewModel
        {
            Id = id,
            FirstName = "Test DoctorName",
            LastName = "Test LastName",
            MiddleName = "Test MiddleName",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Email = "test@email",
            Specialization = "TestSpec",
            Office = "TestOffice",
            CareerStartYear = 2000,
            Status = DoctorStatus.AtWork
        };
        var en = new DoctorEntity
        {
            Id = id,
            FirstName = "Test DoctorName",
            LastName = "Test LastName",
            MiddleName = "Test MiddleName",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Email = "test@email",
            Specialization = "TestSpec",
            Office = "TestOffice",
            CareerStartYear = 2000,
            Status = DoctorStatus.AtWork
        };
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7105/Doctor");
        request.Content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");
        var actualResult = await Client.SendAsync(request);
        var responseResult = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult.Content.ReadAsStringAsync().Result);

        //Act

        using var request2 = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7105/Doctor?id={responseResult.Id}");
        var actualResult2 = await Client.SendAsync(request2);
        var responseResult2 = JsonConvert.DeserializeObject<DoctorViewModel>(actualResult2.Content.ReadAsStringAsync().Result);

        //Assert
        Assert.Equal(HttpStatusCode.OK, actualResult2.StatusCode);
        Assert.Equivalent(responseResult2, viewModel);
    }
}
