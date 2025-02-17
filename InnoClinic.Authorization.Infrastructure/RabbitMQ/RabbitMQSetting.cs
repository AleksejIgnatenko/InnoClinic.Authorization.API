namespace InnoClinic.Authorization.Infrastructure.RabbitMQ
{
    /// <summary>
    /// Represents the configuration settings for RabbitMQ connection.
    /// </summary>
    public class RabbitMQSetting
    {
        /// <summary>
        /// Gets or sets the hostname of the RabbitMQ server.
        /// </summary>
        public string HostName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username for authenticating with the RabbitMQ server.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for authenticating with the RabbitMQ server.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
