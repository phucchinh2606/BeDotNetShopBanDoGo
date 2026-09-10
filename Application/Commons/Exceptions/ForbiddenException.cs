using System.Net;

namespace Application.Commons.Exceptions
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message = "Bạn không có quyền thực hiện thao tác này.")
            : base(message, HttpStatusCode.Forbidden)
        {
        }
    }
}
