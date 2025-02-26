using InnoClinic.Authorization.Core.Models.DoctorModels;
using InnoClinic.Authorization.DataAccess.Repositories;

namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Provides services for managing doctors.
    /// </summary>
    public class DoctorService : IDoctorService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IDoctorRepository _doctorRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorService"/> class.
        /// </summary>
        /// <param name="doctorRepository">The repository for doctor entities.</param>
        /// <param name="accountRepository">The repository for account entities.</param>
        public DoctorService(IDoctorRepository doctorRepository, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _doctorRepository = doctorRepository;
        }

        /// <summary>
        /// Creates a new doctor asynchronously.
        /// </summary>
        /// <param name="accountId">The identifier of the associated account.</param>
        /// <param name="doctor">The doctor entity to create.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CreateDoctorAsync(Guid accountId, DoctorEntity doctor)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            doctor.Account = account;

            await _doctorRepository.CreateAsync(doctor);
        }
    }
}
