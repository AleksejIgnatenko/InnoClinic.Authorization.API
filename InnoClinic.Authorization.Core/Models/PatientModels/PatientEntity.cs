using InnoClinic.Authorization.Core.Models.AccountModels;

namespace InnoClinic.Appointments.Core.Models.PatientModels
{
    public class PatientEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public AccountEntity? Account { get; set; }
    }
}
