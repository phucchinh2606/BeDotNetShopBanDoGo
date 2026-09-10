using System.Net;

namespace Application.Commons.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Bạn chưa đăng nhập hoặc phiên làm việc đã hết hạn.")
            : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}
