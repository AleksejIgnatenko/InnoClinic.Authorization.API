namespace InnoClinic.Authorization.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when there is an error related to data processing.
    /// </summary>
    public class ExceptionWithStatusCode : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with the data exception.
        /// </summary>
        public int HttpStatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionWithStatusCode"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="httpStatusCode">The HTTP status code associated with this exception.</param>
        public ExceptionWithStatusCode(string message, int httpStatusCode)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}