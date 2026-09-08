using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commons.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        // Constructor cho trường hợp thành công có dữ liệu
        public ApiResponse(T data, string message = "Thành công")
        {
            Success = true;
            Message = message;
            Data = data;
        }

        // Constructor cho trường hợp chỉ cần trả về thông báo (Thành công/Thất bại)
        public ApiResponse(bool success, string message)
        {
            Success = success;
            Message = message;
            Data = default;
        }

        // Các Helper method tĩnh để code Controller ngắn gọn hơn
        public static ApiResponse<T> SuccessResult(T data, string message = "Thành công")
        {
            return new ApiResponse<T>(data, message);
        }

        public static ApiResponse<T> FailureResult(string message)
        {
            return new ApiResponse<T>(false, message);
        }
    }
}
