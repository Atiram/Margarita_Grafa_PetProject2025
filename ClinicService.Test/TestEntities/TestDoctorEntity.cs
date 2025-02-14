using ClinicService.DAL.Entities;
using ClinicService.DAL.Entities.Enums;

namespace ClinicService.Test.TestEntities;
public static class TestDoctorEntity
{
    public static DoctorEntity Doctor => new()
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
}