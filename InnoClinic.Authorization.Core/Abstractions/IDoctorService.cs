using InnoClinic.Authorization.Core.Models.DoctorModels;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IDoctorService
    {
        Task CreateDoctorAsync(Guid accountId, DoctorEntity doctor);
    }
}