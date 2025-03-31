using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Authorization.DataAccess.Repositories;

namespace InnoClinic.Authorization.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAccountRepository _accountRepository;

        public PatientService(IPatientRepository patientRepository, IAccountRepository accountRepository)
        {
            _patientRepository = patientRepository;
            _accountRepository = accountRepository;
        }

        public async Task CreatePatientAsync(Guid accountId, PatientEntity patient)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            patient.Account = account;

            await _patientRepository.CreateAsync(patient);
        }
    }
}
