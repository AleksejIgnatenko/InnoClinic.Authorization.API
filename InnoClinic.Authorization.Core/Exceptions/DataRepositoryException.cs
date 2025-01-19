namespace InnoClinic.Authorization.Core.Exceptions
{
    public class DataRepositoryException : Exception
    {
        public int HttpStatusCode { get; }
        public DataRepositoryException(string message, int httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}