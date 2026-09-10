using Application.Commons.Exceptions;
using Application.Commons.Models;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi hệ thống: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>(false, exception.Message);

            switch (exception)
            {
                case AppException appEx:
                    // Các lỗi Custom Exception đã được phân loại ở Bước 1
                    context.Response.StatusCode = (int)appEx.StatusCode;
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Không tìm thấy tài nguyên yêu cầu.";
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = "Bạn không có quyền truy cập.";
                    break;

                default:
                    // Lỗi không xác định hoặc crash hệ thống (HTTP 500)
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.";
                    break;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonResult = JsonSerializer.Serialize(response, options);
            return context.Response.WriteAsync(jsonResult);
        }
    }
}
