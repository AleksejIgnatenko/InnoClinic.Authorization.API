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
        public const string ADD_ACCOUNT_QUEUE = "ADD_ACCOUNT_QUEUE";

        /// <summary>
        /// The queue for adding an account in the profile API.
        /// </summary>
        public const string ADD_ACCOUNT_IN_PROFILE_API_QUEUE = "ADD_ACCOUNT_IN_PROFILE_API_QUEUE";

        /// <summary>
        /// The queue for updating an account's phone number.
        /// </summary>
        public const string UPDATE_ACCOUNT_PHONE_QUEUE = "UPDATE_ACCOUNT_PHONE_QUEUE";

        /// <summary>
        /// The queue for updating an existing account.
        /// </summary>
        public const string UPDATE_ACCOUNT_QUEUE = "UPDATE_ACCOUNT_QUEUE";

        /// <summary>
        /// The queue for deleting an account.
        /// </summary>
        public const string DELETE_ACCOUNT_QUEUE = "DELETE_ACCOUNT_QUEUE";

        /// <summary>
        /// Constant for adding a doctor to the queue.
        /// </summary>
        public const string ADD_DOCTOR_QUEUE = "ADD_DOCTOR_QUEUE";

        /// <summary>
        /// Constant for updating information about a doctor in the queue.
        /// </summary>
        public const string UPDATE_DOCTOR_QUEUE = "UPDATE_DOCTOR_QUEUE";

        /// <summary>
        /// Constant for deleting a doctor from the queue.
        /// </summary>
        public const string DELETE_DOCTOR_QUEUE = "DELETE_DOCTOR_QUEUE";

        /// <summary>
        /// Constant for adding a receptionist to the queue.
        /// </summary>
        public const string ADD_RECEPTIONIST_QUEUE = "ADD_RECEPTIONIST_QUEUE";

        /// <summary>
        /// Constant for updating information about a receptionist in the queue.
        /// </summary>
        public const string UPDATE_RECEPTIONIST_QUEUE = "UPDATE_RECEPTIONIST_QUEUE";

        /// <summary>
        /// Constant for deleting a receptionist from the queue.
        /// </summary>
        public const string DELETE_RECEPTIONIST_QUEUE = "DELETE_RECEPTIONIST_QUEUE";
    }
}
