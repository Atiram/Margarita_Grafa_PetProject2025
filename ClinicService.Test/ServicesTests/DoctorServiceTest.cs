using AutoMapper;
using ClinicService.BLL.Services;
using ClinicService.BLL.Services.Interfaces;
using ClinicService.BLL.Utilities.Mapping;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.Test.TestEntities;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClinicService.Test.ServicesTests;
public class DoctorServiceTest
{
    [Fact]
    public async Task GetDoctorById_Exist_ReturnDoctorModel()
    {
        //Arrange
        var doctorEntity = TestDoctorEntity.NewDoctorEntity;
        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(doctorEntity.Id, CancellationToken.None)).ReturnsAsync(doctorEntity);
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
        var mapper = new Mapper(config);
        var mockDocumentService = new Mock<IDocumentService>();
        mockDocumentService.Setup(service => service.GetPhotoAsync(doctorEntity.Id, CancellationToken.None)).ReturnsAsync("test_url");
        var mockLogger = new Mock<ILogger<DoctorService>>();

        var doctorService = new DoctorService(mockRepository.Object, mockDocumentService.Object, mapper, mockLogger.Object);

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
        mockRepository.Setup(repo => repo.GetByIdAsync(doctorId, CancellationToken.None)).ReturnsAsync((DoctorEntity?)null);
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
        var mapper = new Mapper(config);
        var mockDocumentService = new Mock<IDocumentService>();
        var mockLogger = new Mock<ILogger<DoctorService>>();

        var doctorService = new DoctorService(mockRepository.Object, mockDocumentService.Object, mapper, mockLogger.Object);

        // Act
        var result = await doctorService.GetById(doctorId, CancellationToken.None);

        // Assert
        Assert.Null(result);
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Item with id {doctorId} not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
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
        var mockDocumentService = new Mock<IDocumentService>();
        mockDocumentService.Setup(service => service.UploadPhotoAsync(doctorEntity.Id, createDoctorRequest.Formfile, CancellationToken.None)).Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<DoctorService>>();

        var doctorService = new DoctorService(mockRepository.Object, mockDocumentService.Object, mapper, mockLogger.Object);

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
        var mockDocumentService = new Mock<IDocumentService>();
        mockDocumentService.Setup(service => service.UploadPhotoAsync(doctorEntity.Id, updatedDoctorRequest.Formfile, CancellationToken.None)).Returns(Task.CompletedTask);
        mockDocumentService.Setup(service => service.DeletePhotoAsync(doctorEntity.Id, CancellationToken.None)).Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<DoctorService>>();

        var doctorService = new DoctorService(mockRepository.Object, mockDocumentService.Object, mapper, mockLogger.Object);

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
        var mockDocumentService = new Mock<IDocumentService>();
        mockDocumentService.Setup(service => service.DeletePhotoAsync(doctorEntity.Id, CancellationToken.None)).Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<DoctorService>>();

        var doctorService = new DoctorService(mockRepository.Object, mockDocumentService.Object, mapper, mockLogger.Object);

        //Act
        var result = await doctorService.DeleteAsync(doctorModel.Id, CancellationToken.None);

        //Assert
        Assert.True(result);
    }
}
