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
        var doctorModel = TestDoctorModel.NewDoctorModel;
        var doctorEntity = TestDoctorEntity.NewDoctorEntity;
        doctorEntity.Id = doctorModel.Id;

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(doctorEntity.Id, CancellationToken.None))
                      .ReturnsAsync(doctorEntity);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(doctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.GetById(doctorModel.Id, CancellationToken.None);

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

        var mockMapper = new Mock<IMapper>();
        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await doctorService.GetById(doctorId, CancellationToken.None);

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

        mockRepository.Setup(repo => repo.GetByIdAsync(doctorId, CancellationToken.None))
            .ThrowsAsync(new Exception("Repository Error"));

        var mockMapper = new Mock<IMapper>();
        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await doctorService.GetById(doctorId, CancellationToken.None));
        mockMapper.Verify(m => m.Map<DoctorModel>(It.IsAny<DoctorEntity>()), Times.Never);
    }

    [Fact]
    public async Task CreateDoctor_ValidDoctorModel_ReturnsCreatedDoctorModel()
    {
        //Arrange 
        var doctorModel = TestDoctorModel.NewDoctorModel;
        var doctorEntity = TestDoctorEntity.NewDoctorEntity;
        doctorEntity.Id = doctorModel.Id;

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<DoctorEntity>(), CancellationToken.None)).ReturnsAsync(doctorEntity);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(doctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.CreateAsync(doctorModel, CancellationToken.None);


        //Assert 
        Assert.NotNull(result);
        Assert.Equal(result.Id, doctorEntity.Id);
        Assert.Equal(result.FirstName, doctorEntity.FirstName);
    }

    [Fact]
    public async Task UpdateDoctor_ValidDoctorModel_ReturnsUpdatedDoctorModel()
    {
        //Arrange 
        var doctorModel = TestDoctorModel.NewDoctorModel;
        var updatedDoctorModel = TestDoctorModel.UpdatedDoctorModel;
        var doctorEntity = TestDoctorEntity.UpdatedDoctorEntity;
        updatedDoctorModel.Id = doctorModel.Id;
        doctorEntity.Id = doctorModel.Id;

        var mockRepository = new Mock<IDoctorRepository>();
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<DoctorEntity>(), CancellationToken.None)).ReturnsAsync(doctorEntity);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(updatedDoctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.UpdateAsync(updatedDoctorModel, CancellationToken.None);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal(result.Id, doctorEntity.Id);
        Assert.Equal(result.FirstName, updatedDoctorModel.FirstName);
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

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<DoctorModel>(doctorEntity))
                  .Returns(doctorModel);

        var doctorService = new DoctorService(mockRepository.Object, mockMapper.Object);

        //Act 
        var result = await doctorService.DeleteAsync(doctorModel.Id, CancellationToken.None);

        //Assert 
        Assert.True(result);
    }
}
