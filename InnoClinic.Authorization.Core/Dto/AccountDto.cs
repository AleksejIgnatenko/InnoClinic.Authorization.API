namespace InnoClinic.Authorization.Core.Dto
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid PhotoId { get; set; }
    }
}
