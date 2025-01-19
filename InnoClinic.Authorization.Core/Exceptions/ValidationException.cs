namespace InnoClinic.Authorization.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; }

        public ValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
