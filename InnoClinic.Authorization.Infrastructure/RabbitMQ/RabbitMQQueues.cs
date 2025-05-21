namespace InnoClinic.Authorization.Infrastructure.RabbitMQ
{
    /// <summary>
    /// Defines the names of RabbitMQ queues used in the application.
    /// </summary>
    public class RabbitMQQueues
    {
        /// <summary>
        /// The queue for adding a new account.
        /// </summary>
        public const string ADD_ACCOUNT_QUEUE = "AddAccount";

        /// <summary>
        /// The queue for adding an account in the profile API.
        /// </summary>
        public const string ADD_ACCOUNT_IN_PROFILE_API_QUEUE = "AddAccountInProfileApi";

        /// <summary>
        /// The queue for updating an existing account.
        /// </summary>
        public const string UPDATE_ACCOUNT_PHONE_PHOTO_QUEUE = "UpdateAccountPhonePhoto";
    }
}
