using ClinicService.API.ViewModels;
using ClinicService.DAL.Enums;

namespace ClinicService.Test.TestEntities;
public static class TestDoctorViewModel
{
    public static DoctorViewModel NewDoctorViewModel => new()
    {
        Id = Guid.NewGuid(),
        FirstName = "Test DoctorName",
        LastName = "Test LastName",
        MiddleName = "Test MiddleName",
        DateOfBirth = new DateOnly(1990, 1, 1),
        Email = "test@email",
        Specialization = "TestSpec",
        Office = "TestOffice",
        CareerStartYear = 2000,
        Status = DoctorStatus.AtWork,
        Appointments = new List<AppointmentViewModel>()
    };

    public static DoctorViewModel UpdatedDoctorViewModel => new()
    {
        Id = Guid.NewGuid(),
        FirstName = "ChangedTest DoctorName",
        LastName = "ChangedTest LastName",
        MiddleName = null,
        DateOfBirth = new DateOnly(1999, 1, 1),
        Email = "Ctchangedtest@email",
        Specialization = "ChangedTestSpec",
        Office = "ChangedTestOffice",
        CareerStartYear = 2010,
        Status = DoctorStatus.SickDay,
        Appointments = new List<AppointmentViewModel>()
    };
}
