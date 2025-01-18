namespace InnoClinic.Authorization.Core.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsEmailVerified { get; set; }
        public Guid PhotoId { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string CreateAt { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
        public string UpdateAt { get; set; } = string.Empty;
    }
}
