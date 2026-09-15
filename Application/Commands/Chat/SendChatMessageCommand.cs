using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Commands.Chat
{
    public class SendChatMessageCommand : IRequest<ApiResponse<ChatResponseDto>>
    {
        public Guid? UserId { get; set; }
        public string Message { get; set; }
    }
}
