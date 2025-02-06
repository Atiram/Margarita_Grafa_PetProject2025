namespace ClinicService.DAL.Entities
{
    internal class AppoimentEntity : GenericEntity
    {
        public int AppoimentId { get; set; }
        public DoctorEntity Doctor { get; set; }
        public PatientEntity Patient { get; set; }
        public DateTime Date { get; set; }
        public DateTime Slots { get; set; }

    }
}
