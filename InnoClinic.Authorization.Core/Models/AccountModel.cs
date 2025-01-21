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
        public DateTime CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
