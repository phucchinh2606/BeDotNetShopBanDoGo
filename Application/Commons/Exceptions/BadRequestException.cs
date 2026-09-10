using System.Net;

namespace Application.Commons.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}
