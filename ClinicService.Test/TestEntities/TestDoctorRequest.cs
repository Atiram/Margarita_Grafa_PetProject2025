using ClinicService.BLL.Models.Requests;
using ClinicService.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ClinicService.Test.TestEntities;
public static class TestDoctorRequest
{
    public static CreateDoctorRequest NewCreateDoctorRequest
    {
        get
        {
            var mockFile = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

            mockFile.Setup(f => f.FileName).Returns("test.jpg");
            mockFile.Setup(f => f.Length).Returns(stream.Length);
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");

            return new CreateDoctorRequest
            {
                FirstName = "Test DoctorName",
                LastName = "Test LastName",
                MiddleName = "Test MiddleName",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Email = "test@email",
                Specialization = "TestSpec",
                Office = "TestOffice",
                CareerStartYear = 2000,
                Status = DoctorStatus.AtWork,
                Formfile = mockFile.Object
            };
        }
    }

    public static UpdateDoctorRequest UpdatedDoctorRequest => new()
    {
        Id = Guid.NewGuid(),
        FirstName = "ChangedTest DoctorName",
        LastName = "ChangedTest LastName",
        MiddleName = "ChangedTest MiddleName",
        DateOfBirth = new DateOnly(1999, 1, 1),
        Email = "ChangedTest@email",
        Specialization = "ChangedTestSpec",
        Office = "ChangedTestOffice",
        CareerStartYear = 2010,
        Status = DoctorStatus.SickDay,
    };
}
