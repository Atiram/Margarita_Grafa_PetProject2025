namespace ClinicService.DAL.Entities
{
    public class DoctorEntity : GenericEntity
    {
        public int DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Specialization { get; set; }
        public string Office { get; set; }
        public int CareerStartYear { get; set; }
        public string Status { get; set; }
    }
}
