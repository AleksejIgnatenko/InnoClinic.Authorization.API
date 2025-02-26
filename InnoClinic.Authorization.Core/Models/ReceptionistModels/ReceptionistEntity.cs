using InnoClinic.Authorization.Core.Models.AccountModels;

namespace InnoClinic.Authorization.Core.Models.ReceptionistModels
{
    public class ReceptionistEntity
    {
        public Guid Id { get; set; }
        public AccountEntity Account { get; set; } = new AccountEntity();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
