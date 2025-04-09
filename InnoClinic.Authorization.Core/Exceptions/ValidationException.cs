namespace InnoClinic.Authorization.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when validation of input fails.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Gets a dictionary of validation errors that occurred during input validation.
        /// </summary>
        public Dictionary<string, string> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="errors">A dictionary containing the validation errors.</param>
        public ValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}