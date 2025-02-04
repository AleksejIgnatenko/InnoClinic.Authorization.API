using System.Text;
using InnoClinic.Authorization.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace InnoClinic.Authorization.Application.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly RabbitMQSetting _rabbitMqSetting;
        private readonly ConnectionFactory _factory;

        public RabbitMQService(IOptions<RabbitMQSetting> rabbitMqSetting)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;

            _factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };
        }

        public async Task CreateQueuesAsync()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                await Task.Run(() =>
                {
                    channel.QueueDeclare(RabbitMQQueues.ADD_ACCOUNT_QUEUE, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(RabbitMQQueues.UPDATE_ACCOUNT_QUEUE, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(RabbitMQQueues.DELETE_ACCOUNT_QUEUE, durable: false, exclusive: false, autoDelete: false, arguments: null);
                });
            }
        }

        public async Task PublishMessageAsync(object obj, string queueName)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var messageJson = JsonConvert.SerializeObject(obj);
            var body = Encoding.UTF8.GetBytes(messageJson);

            await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body));
        }
    }
}

