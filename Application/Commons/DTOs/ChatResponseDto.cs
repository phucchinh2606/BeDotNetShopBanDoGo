namespace Application.Commons.DTOs
{
    public class ChatResponseDto
    {
        public string? SessionId { get; set; } // Trả về cho Frontend lưu trữ
        public string Response { get; set; }
    }
}
