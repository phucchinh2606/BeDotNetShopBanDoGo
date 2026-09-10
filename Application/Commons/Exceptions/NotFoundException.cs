using System.Net;

namespace Application.Commons.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message)
            : base(message, HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Không tìm thấy {name} với mã ({key}).", HttpStatusCode.NotFound)
        {
        }
    }
}
