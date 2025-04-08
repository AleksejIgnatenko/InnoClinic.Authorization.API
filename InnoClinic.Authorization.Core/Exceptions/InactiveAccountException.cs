namespace InnoClinic.Authorization.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an account is inactive.
    /// </summary>
    public class InactiveAccountException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with the inactive account exception.
        /// </summary>
        public int HttpStatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InactiveAccountException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="httpStatusCode">The HTTP status code associated with this exception.</param>
        public InactiveAccountException(string message, int httpStatusCode)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}