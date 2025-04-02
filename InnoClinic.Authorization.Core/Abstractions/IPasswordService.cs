namespace InnoClinic.Authorization.Application.Services
{
    public interface IPasswordService
    {
        bool Verify(string password, string hashedPassword);
    }
}