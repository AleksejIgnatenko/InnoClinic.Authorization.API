namespace InnoClinic.Authorization.Application.Services
{
    public class PasswordService : IPasswordService
    {
        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
