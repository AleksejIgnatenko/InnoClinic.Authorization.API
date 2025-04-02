namespace InnoClinic.Authorization.Core.Exceptions
{
    public class DataException : Exception
    {
        public int HttpStatusCode { get; }
        public DataException(string message, int httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}