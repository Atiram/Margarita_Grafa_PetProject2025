using System.Net;
using System.Text.Json;
using AutoMapper;
using ClinicService.BLL.Services;
using ClinicService.BLL.Utilities.Mapping;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.Test.TestEntities;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace ClinicService.Test.ServicesTests;
public class DoctorServiceTest
{
    [Fact]
    public async Task GetDoctorById_Exist_ReturnDoctorModel()
    {
        //Arrange 
        var doctorEntity = TestDoctorEntity.NewDoctorEntity;

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(doctorEntity.Id, CancellationToken.None))
                      .ReturnsAsync(doctorEntity);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
        var mapper = new Mapper(config);
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("FileServiceBaseUrl").Value)
            .Returns("https://localhost:7049/api/File");

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new { storageLocation = "test_url" }))
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        var doctorService = new DoctorService(mockRepository.Object, mapper, configurationMock.Object, mockHttpClientFactory.Object);

        //Act 
        var result = await doctorService.GetById(doctorEntity.Id, CancellationToken.None);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal(doctorEntity.Id, result.Id);
        Assert.Equal(result.FirstName, doctorEntity.FirstName);
        Assert.Equal(result.Office, doctorEntity.Office);
    }

    [Fact]
    public async Task GetById_DoctorNotFound_ReturnsNull()
    {
        // Arrange
        var doctorId = Guid.NewGuid();

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(doctorId, CancellationToken.None))
                      .ReturnsAsync((DoctorEntity?)null);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
        var mapper = new Mapper(config);
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("FileServiceBaseUrl").Value)
            .Returns("https://localhost:7049/api/File");

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(new HttpClient());

        var doctorService = new DoctorService(mockRepository.Object, mapper, configurationMock.Object, mockHttpClientFactory.Object);

        // Act
        var result = await doctorService.GetById(doctorId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetById_ExceptionFromRepository_RethrowsException()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var mockRepository = new Mock<IDoctorRepository>();

        mockRepository.Setup(repo => repo.GetByIdAsync(doctorId, CancellationToken.None))
            .ThrowsAsync(new Exception("Repository Error"));

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
        var mapper = new Mapper(config);
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("FileServiceBaseUrl").Value)
            .Returns("https://localhost:7049/api/File");
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(new HttpClient());

        var doctorService = new DoctorService(mockRepository.Object, mapper, configurationMock.Object, mockHttpClientFactory.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await doctorService.GetById(doctorId, CancellationToken.None));
    }

    [Fact]
    public async Task CreateDoctor_ValidDoctorModel_ReturnsCreatedDoctorModel()
    {
        //Arrange 
        var createDoctorRequest = TestDoctorRequest.NewCreateDoctorRequest;
        var doctorEntity = TestDoctorEntity.NewDoctorEntity;

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<DoctorEntity>(), CancellationToken.None)).ReturnsAsync(doctorEntity);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
        var mapper = new Mapper(config);
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("FileServiceBaseUrl").Value)
            .Returns("https://localhost:7049/api/File");
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new { id = Guid.NewGuid().ToString(), storageLocation = "test_upload_url" })),
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        var doctorService = new DoctorService(mockRepository.Object, mapper, configurationMock.Object, mockHttpClientFactory.Object);

        //Act
        var result = await doctorService.CreateAsync(createDoctorRequest, CancellationToken.None);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal(result.Id, doctorEntity.Id);
        Assert.Equal(result.FirstName, doctorEntity.FirstName);
    }

    [Fact]
    public async Task UpdateDoctor_ValidDoctorModel_ReturnsUpdatedDoctorModel()
    {
        //Arrange
        var doctorEntity = TestDoctorEntity.UpdatedDoctorEntity;
        var updatedDoctorRequest = TestDoctorRequest.UpdatedDoctorRequest();

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<DoctorEntity>(), CancellationToken.None)).ReturnsAsync(doctorEntity);
        mockRepository.Setup(repo => repo.GetByIdAsync(updatedDoctorRequest.Id, CancellationToken.None)).ReturnsAsync(doctorEntity);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
        var mapper = new Mapper(config);
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("FileServiceBaseUrl").Value)
            .Returns("https://localhost:7049/api/File");

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new { id = Guid.NewGuid().ToString(), storageLocation = "test_upload_url" })),
        });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        var doctorService = new DoctorService(mockRepository.Object, mapper, configurationMock.Object, mockHttpClientFactory.Object);

        //Act
        var result = await doctorService.UpdateAsync(updatedDoctorRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, doctorEntity.Id);
        Assert.Equal(result.FirstName, doctorEntity.FirstName);
    }

    [Fact]
    public async Task DeleteDoctor_ValidDoctorModel_ReturnsTrue()
    {
        //Arrange 
        var doctorModel = TestDoctorModel.NewDoctorModel;
        var doctorEntity = TestDoctorEntity.NewDoctorEntity;
        doctorEntity.Id = doctorModel.Id;
        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.DeleteAsync(doctorEntity.Id, CancellationToken.None)).ReturnsAsync(true);
        mockRepository.Setup(repo => repo.GetByIdAsync(doctorEntity.Id, CancellationToken.None)).ReturnsAsync(doctorEntity);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
        var mapper = new Mapper(config);
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("FileServiceBaseUrl").Value)
            .Returns("https://localhost:7049/api/File");
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new { storageLocation = "test_url" })),
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        var doctorService = new DoctorService(mockRepository.Object, mapper, configurationMock.Object, mockHttpClientFactory.Object);

        //Act
        var result = await doctorService.DeleteAsync(doctorModel.Id, CancellationToken.None);

        //Assert 
        Assert.True(result);
    }
}
