using System.Text;
using AutoMapper;
using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Enums;
using InnoClinic.Authorization.Core.Models.AccountModels;
using InnoClinic.Authorization.Core.Models.DoctorModels;
using InnoClinic.Authorization.Core.Models.ReceptionistModels;
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
        private readonly IDoctorService _doctorService;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IReceptionistRepository _receptionistRepository;
        private readonly IReceptionistService _receptionistService;

        public RabbitMQListener(IOptions<RabbitMQSetting> rabbitMqSetting, IMapper mapper, IAccountRepository accountRepository, IDoctorService doctorService, IDoctorRepository doctorRepository, IReceptionistRepository receptionistRepository, IReceptionistService receptionistService)
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
            _doctorRepository = doctorRepository;
            _doctorService = doctorService;
            _receptionistRepository = receptionistRepository;
            _receptionistService = receptionistService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            #region account
            var addAccountPhoneNumberConsumer = new EventingBasicConsumer(_channel);
            addAccountPhoneNumberConsumer.Received += async (ch, ea) =>
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
            _channel.BasicConsume(RabbitMQQueues.ADD_ACCOUNT_IN_PROFILE_API_QUEUE, false, addAccountPhoneNumberConsumer);

            var updateAccountPhoneNumberConsumer = new EventingBasicConsumer(_channel);
            updateAccountPhoneNumberConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var accountDto = JsonConvert.DeserializeObject<AccountDto>(content);
                var account = _mapper.Map<AccountEntity>(accountDto);

                await _accountRepository.UpdateAsync(account.Id, account.PhoneNumber);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.UPDATE_ACCOUNT_PHONE_QUEUE, false, updateAccountPhoneNumberConsumer);

            #endregion

            #region doctor
            var addDoctorConsumer = new EventingBasicConsumer(_channel);
            addDoctorConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var doctorDto = JsonConvert.DeserializeObject<DoctorDto>(content);
                var doctor = _mapper.Map<DoctorEntity>(doctorDto);

                await _doctorService.CreateDoctorAsync(doctorDto.AccountId, doctor);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.ADD_DOCTOR_QUEUE, false, addDoctorConsumer);

            var updateDoctorConsumer = new EventingBasicConsumer(_channel);
            updateDoctorConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var doctorDto = JsonConvert.DeserializeObject<DoctorDto>(content);
                var doctor = _mapper.Map<DoctorEntity>(doctorDto);

                await _doctorRepository.UpdateAsync(doctor);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.UPDATE_DOCTOR_QUEUE, false, updateDoctorConsumer);

            var deleteDoctorConsumer = new EventingBasicConsumer(_channel);
            deleteDoctorConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var doctorDto = JsonConvert.DeserializeObject<DoctorDto>(content);
                var doctor = _mapper.Map<DoctorEntity>(doctorDto);

                await _doctorRepository.DeleteAsync(doctor);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.DELETE_DOCTOR_QUEUE, false, deleteDoctorConsumer);

            #endregion

            #region receptionist
            var addReceptionistConsumer = new EventingBasicConsumer(_channel);
            addReceptionistConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var receptionistDto = JsonConvert.DeserializeObject<ReceptionistDto>(content);
                var receptionist = _mapper.Map<ReceptionistEntity>(receptionistDto);

                await _receptionistService.CreateReceptionistAsync(receptionistDto.AccountId, receptionist);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.ADD_RECEPTIONIST_QUEUE, false, addReceptionistConsumer);

            var updateReceptionistConsumer = new EventingBasicConsumer(_channel);
            updateReceptionistConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var receptionistDto = JsonConvert.DeserializeObject<ReceptionistDto>(content);
                var receptionist = _mapper.Map<ReceptionistEntity>(receptionistDto);

                await _receptionistRepository.UpdateAsync(receptionist);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.UPDATE_RECEPTIONIST_QUEUE, false, updateReceptionistConsumer);

            var deleteReceptionistConsumer = new EventingBasicConsumer(_channel);
            deleteReceptionistConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var receptionistDto = JsonConvert.DeserializeObject<ReceptionistDto>(content);
                var receptionist = _mapper.Map<ReceptionistEntity>(receptionistDto);

                await _receptionistRepository.DeleteAsync(receptionist);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.DELETE_RECEPTIONIST_QUEUE, false, deleteReceptionistConsumer);

            #endregion

            return Task.CompletedTask;
        }
    }
}
