using InnoClinic.Authorization.Core.Models.ReceptionistModels;
using InnoClinic.Authorization.DataAccess.Repositories;

namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Сервис для управления данными рецепционистов.
    /// </summary>
    public class ReceptionistService : IReceptionistService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IReceptionistRepository _receptionistRepository;

        /// <summary>
        /// Конструктор для инициализации сервисов.
        /// </summary>
        /// <param name="accountRepository">Репозиторий аккаунтов.</param>
        /// <param name="receptionistRepository">Репозиторий рецепционистов.</param>
        public ReceptionistService(IAccountRepository accountRepository, IReceptionistRepository receptionistRepository)
        {
            _accountRepository = accountRepository;
            _receptionistRepository = receptionistRepository;
        }

        /// <summary>
        /// Создает нового рецепциониста, связанного с указанным аккаунтом.
        /// </summary>
        /// <param name="accountId">Идентификатор аккаунта.</param>
        /// <param name="receptionist">Объект рецепциониста для создания.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public async Task CreateReceptionistAsync(Guid accountId, ReceptionistEntity receptionist)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            receptionist.Account = account;

            await _receptionistRepository.CreateAsync(receptionist);
        }
    }
}
