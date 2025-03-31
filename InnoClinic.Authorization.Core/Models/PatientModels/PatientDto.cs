namespace InnoClinic.Appointments.Core.Models.PatientModels
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public Guid AccountId { get; set; }
    }
}
