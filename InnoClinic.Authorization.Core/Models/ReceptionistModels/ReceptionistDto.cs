namespace InnoClinic.Authorization.Core.Models.ReceptionistModels
{
    public class ReceptionistDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
