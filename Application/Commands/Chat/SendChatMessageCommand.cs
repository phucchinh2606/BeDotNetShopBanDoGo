using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Commands.Chat
{
    public class SendChatMessageCommand : IRequest<ApiResponse<ChatResponseDto>>
    {
        public string? SessionId { get; set; } // Nhận SessionId từ client (nếu có)
        public Guid? UserId { get; set; }
        public string Message { get; set; }
    }
}
