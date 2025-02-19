using System.Text;
using AutoMapper;
using InnoClinic.Authorization.Core.Dto;
using InnoClinic.Authorization.Core.Enums;
using InnoClinic.Authorization.Core.Models;
using InnoClinic.Authorization.DataAccess.Repositories;
using InnoClinic.Authorization.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InnoClinic.Authorization.Application.RabbitMQ
{
    public class RabbitMQListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly RabbitMQSetting _rabbitMqSetting;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public RabbitMQListener(IOptions<RabbitMQSetting> rabbitMqSetting, IMapper mapper, IAccountRepository accountRepository)
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
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var addAccountPhoneNumberConsumer = new EventingBasicConsumer(_channel);
            addAccountPhoneNumberConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var accountDto = JsonConvert.DeserializeObject<AccountDto>(content);
                var account = _mapper.Map<AccountModel>(accountDto);
                account.IsEmailVerified = true;
                account.CreateAt = DateTime.UtcNow;
                account.CreateBy = RoleEnum.Receptionist;

                await _accountRepository.CreateAsync(account);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.ADD_ACCOUNT_IN_PROFILE_API_QUEUE, false, addAccountPhoneNumberConsumer);

            var updateAccountPhoneNumberConsumer = new EventingBasicConsumer(_channel);
            updateAccountPhoneNumberConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var accountDto = JsonConvert.DeserializeObject<AccountDto>(content);
                var account = _mapper.Map<AccountModel>(accountDto);

                await _accountRepository.UpdateAsync(account.Id, account.PhoneNumber);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.UPDATE_ACCOUNT_PHONE_QUEUE, false, updateAccountPhoneNumberConsumer);

            return Task.CompletedTask;
        }
    }
}
