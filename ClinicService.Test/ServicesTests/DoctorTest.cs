using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Entities.Enums;
using ClinicService.DAL.Repositories.Interfaces;
using Moq;

namespace ClinicService.Test.ServicesTests;
public class DoctorTest
{
    [Fact]
    public async Task GetDoctorById_Exist_ReturnDoctorModel()
    {
        //Arrange 
        var doctorId = Guid.NewGuid();
        var doctorEntity = new DoctorEntity
        {
            Id = doctorId,
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
        var doctorModel = new DoctorModel
        {
            Id = doctorId,
            FirstName = "Test DoctorName",
            LastName = "Test LastName",
            MiddleName = "Test MiddleName",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Email = "teat@email",
            Specialization = "TestSpec",
            Office = "TestOffice",
            CareerStartYear = 2000,
            Status = DoctorStatus.AtWork
        };

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(doctorId))
                      .ReturnsAsync(doctorEntity);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(doctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.GetById(doctorId);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal(doctorId, result.Id);
        Assert.Equal("Test DoctorName", result.FirstName);
        Assert.Equal("TestOffice", result.Office);
    }

    [Fact]
    public async Task GetById_DoctorNotFound_ReturnsNull()
    {
        // Arrange
        var doctorId = Guid.NewGuid();

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(doctorId))
                      .ReturnsAsync((DoctorEntity)null);

        var mockMapper = new Mock<IMapper>();

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await doctorService.GetById(doctorId);

        // Assert
        Assert.Null(result);
        mockMapper.Verify(m => m.Map<DoctorModel>(It.IsAny<DoctorEntity>()), Times.AtMostOnce);
    }
    [Fact]
    public async Task GetById_ExceptionFromRepository_RethrowsException()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var mockRepository = new Mock<IDoctorRepository>();

        mockRepository.Setup(repo => repo.GetByIdAsync(doctorId))
            .ThrowsAsync(new Exception("Repository Error"));

        var mockMapper = new Mock<IMapper>();

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await doctorService.GetById(doctorId));

        mockMapper.Verify(m => m.Map<DoctorModel>(It.IsAny<DoctorEntity>()), Times.Never);
    }

    [Fact]
    public async Task CreateDoctor_ValidDoctorModel_ReturnsCreatedDoctorModel()
    {
        //Arrange 
        var doctorId = Guid.NewGuid();
        var doctorEntity = new DoctorEntity { Id = doctorId, FirstName = "Test Doctor" };
        var doctorModel = new DoctorModel { Id = doctorId, FirstName = "Test Doctor" };

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<DoctorEntity>())).ReturnsAsync(doctorEntity);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(doctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.CreateAsync(doctorModel);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal(doctorId, result.Id);
        Assert.Equal("Test Doctor", result.FirstName);
    }
    [Fact]
    public async Task UpdateDoctor_ValidDoctorModel_ReturnsUpdatedDoctorModel()
    {
        //Arrange 
        var doctorId = Guid.NewGuid();
        var doctorEntity = new DoctorEntity { Id = doctorId, FirstName = "Test Doctor" };
        var doctorModel = new DoctorModel { Id = doctorId, FirstName = "Test Doctor" };

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<DoctorEntity>())).ReturnsAsync(doctorEntity);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(doctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.UpdateAsync(doctorModel);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal(doctorId, result.Id);
        Assert.Equal("Test Doctor", result.FirstName);
    }
    [Fact]
    public async Task DeleteDoctor_ValidDoctorModel_ReturnsTrue()
    {
        //Arrange 
        var doctorId = Guid.NewGuid();
        var doctorEntity = new DoctorEntity { Id = doctorId, FirstName = "Test Doctor" };
        var doctorModel = new DoctorModel { Id = doctorId, FirstName = "Test Doctor" };

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.DeleteAsync(doctorId)).ReturnsAsync(true);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(doctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.DeleteAsync(doctorModel.Id);

        //Assert 
        Assert.True(result);
    }
}
