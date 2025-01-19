using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IValidationService
    {
        Dictionary<string, string> AccountValidation(AccountModel accountModel);
    }
}