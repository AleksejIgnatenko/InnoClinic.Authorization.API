using System.Text;
using AutoMapper;
using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Enums;
using InnoClinic.Authorization.Core.Models.AccountModels;
using InnoClinic.Authorization.DataAccess.Repositories;
using InnoClinic.Authorization.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InnoClinic.Authorization.Application.RabbitMQ
{
    /// <summary>
    /// Listens for messages from RabbitMQ and processes account-related events.
    /// </summary>
    public class RabbitMQListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly RabbitMQSetting _rabbitMqSetting;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountService _accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQListener"/> class.
        /// </summary>
        /// <param name="rabbitMqSetting">The RabbitMQ settings.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        /// <param name="accountRepository">The repository for account data access.</param>
        /// <param name="accountService">The service for account operations.</param>
        public RabbitMQListener(IOptions<RabbitMQSetting> rabbitMqSetting, IMapper mapper, IAccountRepository accountRepository, IAccountService accountService)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _mapper = mapper;
            _accountRepository = accountRepository;
            _accountService = accountService;
        }

        /// <summary>
        /// Executes the background service to listen for RabbitMQ messages.
        /// </summary>
        /// <param name="stoppingToken">A token that can be used to signal cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            #region account
            var addAccountConsumer = new EventingBasicConsumer(_channel);
            addAccountConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var accountDto = JsonConvert.DeserializeObject<AccountDto>(content);
                var account = _mapper.Map<AccountEntity>(accountDto);
                account.IsEmailVerified = true;
                account.CreateAt = DateTime.UtcNow;
                account.CreateBy = RoleEnum.Receptionist.ToString();

                await _accountRepository.CreateAsync(account);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.ADD_ACCOUNT_IN_PROFILE_API_QUEUE, false, addAccountConsumer);

            var updateAccountPhoneNumberConsumer = new EventingBasicConsumer(_channel);
            updateAccountPhoneNumberConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var accountDto = JsonConvert.DeserializeObject<AccountUpdatePhonePhotoDto>(content);

                await _accountService.UpdatePhonePhotoInAccountAsync(accountDto.Id, accountDto.PhoneNumber, accountDto.PhotoId);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.UPDATE_ACCOUNT_PHONE_PHOTO_QUEUE, false, updateAccountPhoneNumberConsumer);
            #endregion

            return Task.CompletedTask;
        }
    }
}