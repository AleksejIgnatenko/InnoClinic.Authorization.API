using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IValidationService
    {
        Dictionary<string, string> Validation(AccountModel accountModel);
    }
}