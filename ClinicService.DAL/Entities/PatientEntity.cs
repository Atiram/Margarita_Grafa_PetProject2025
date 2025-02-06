namespace ClinicService.DAL.Entities
{
    internal class PatientEntity : GenericEntity
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; } = string.Empty;
        public int PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
