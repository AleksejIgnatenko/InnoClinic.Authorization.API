namespace InnoClinic.Authorization.Core.Exceptions
{
    public class InactiveAccountException : Exception
    {
        public int HttpStatusCode { get; }

        public InactiveAccountException(string message, int httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
