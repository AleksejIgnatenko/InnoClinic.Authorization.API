using InnoClinic.Authorization.Core.Models.ReceptionistModels;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IReceptionistService
    {
        Task CreateReceptionistAsync(Guid accountId, ReceptionistEntity receptionist);
    }
}