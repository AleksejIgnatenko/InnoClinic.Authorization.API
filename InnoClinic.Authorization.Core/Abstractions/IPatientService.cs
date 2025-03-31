using InnoClinic.Appointments.Core.Models.PatientModels;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IPatientService
    {
        Task CreatePatientAsync(Guid accountId, PatientEntity patient);
    }
}