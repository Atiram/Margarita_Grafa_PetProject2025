using ClinicService.BLL.Models.Requests;
using ClinicService.DAL.Enums;

namespace ClinicService.Test.TestEntities;
public static class TestDoctorRequest
{
    public static CreateDoctorRequest NewCreateDoctorRequest => new()
    {
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

    public static UpdateDoctorRequest UpdatedDoctorRequest => new()
    {
        Id = Guid.NewGuid(),
        FirstName = "ChangedTest DoctorName",
        LastName = "ChangedTest LastName",
        MiddleName = "ChangedTest MiddleName",
        DateOfBirth = new DateOnly(1999, 1, 1),
        Email = "Ctchangedtest@email",
        Specialization = "ChangedTestSpec",
        Office = "ChangedTestOffice",
        CareerStartYear = 2010,
        Status = DoctorStatus.SickDay,
    };
}
