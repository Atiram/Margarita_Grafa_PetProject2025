using AutoMapper;
using ClinicService.BLL.Models;
using ClinicService.BLL.Services;
using ClinicService.DAL.Entities;
using ClinicService.DAL.Repositories.Interfaces;
using ClinicService.Test.TestEntities;
using Moq;

namespace ClinicService.Test.ServicesTests;
public class DoctorTest
{
    [Fact]
    public async Task GetDoctorById_Exist_ReturnDoctorModel()
    {
        //Arrange 
        var doctorModel = TestDoctorModel.DoctorModel;
        var doctorEntity = TestDoctorEntity.DoctorEntity;
        doctorEntity.Id = doctorModel.Id;

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(doctorEntity.Id))
                      .ReturnsAsync(doctorEntity);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(doctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.GetById(doctorModel.Id);

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
        var doctorModel = TestDoctorModel.DoctorModel;
        var doctorEntity = TestDoctorEntity.DoctorEntity;
        doctorEntity.Id = doctorModel.Id;

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
        Assert.Equal(result.Id, doctorEntity.Id);
        Assert.Equal(result.FirstName, doctorEntity.FirstName);
    }
    [Fact]
    public async Task UpdateDoctor_ValidDoctorModel_ReturnsUpdatedDoctorModel()
    {
        //Arrange 
        var doctorModel = TestDoctorModel.DoctorModel;
        var updatedDoctorModel = TestDoctorModel.UpdatedDoctorModel;
        var doctorEntity = TestDoctorEntity.DoctorEntity;
        updatedDoctorModel.Id = doctorModel.Id;
        doctorEntity.Id = doctorModel.Id;

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<DoctorEntity>())).ReturnsAsync(doctorEntity);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(doctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.UpdateAsync(updatedDoctorModel);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal(result.Id, doctorEntity.Id);
        Assert.Equal(result.FirstName, updatedDoctorModel.FirstName);
    }
    [Fact]
    public async Task DeleteDoctor_ValidDoctorModel_ReturnsTrue()
    {
        //Arrange 
        var doctorModel = TestDoctorModel.DoctorModel;
        var doctorEntity = TestDoctorEntity.DoctorEntity;
        doctorEntity.Id = doctorModel.Id;

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.DeleteAsync(doctorEntity.Id)).ReturnsAsync(true);

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
