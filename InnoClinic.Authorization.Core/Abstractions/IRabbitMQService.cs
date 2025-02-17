
namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Defines the operations for interacting with RabbitMQ services.
    /// </summary>
    public interface IRabbitMQService
    {
        /// <summary>
        /// Asynchronously publishes a message to the specified RabbitMQ queue.
        /// </summary>
        /// <param name="obj">The message object to publish.</param>
        /// <param name="queueName">The name of the queue to which the message will be published.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task PublishMessageAsync(object obj, string queueName);

        /// <summary>
        /// Asynchronously creates the necessary queues in RabbitMQ.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateQueuesAsync();
    }
}