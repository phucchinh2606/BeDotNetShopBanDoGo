using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Application.Commands.Chat
{
    public class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, ApiResponse<ChatResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeminiService _geminiService;
        private readonly IMemoryCache _cache;

        public SendChatMessageCommandHandler(IUnitOfWork unitOfWork, IGeminiService geminiService, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _geminiService = geminiService;
            _cache = cache;
        }

        public async Task<ApiResponse<ChatResponseDto>> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return ApiResponse<ChatResponseDto>.FailureResult("Nội dung tin nhắn không được để trống.");
            }

            // 1. Lấy context sản phẩm từ MemoryCache
            string contextData = await _cache.GetOrCreateAsync("AI_PRODUCT_CONTEXT", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                var products = await _unitOfWork.Products.GetAllAsync();
                var sb = new StringBuilder();

                foreach (var p in products)
                {
                    sb.AppendLine($"- {p.ProductName} | Giá: {p.Price:N0}đ | Kích thước: {p.Dimensions} | Chất liệu: {p.Material}");
                }
                return sb.ToString();
            });

            // 2. Gọi Gemini Service
            var botReply = await _geminiService.GenerateChatResponseAsync(request.Message, contextData, cancellationToken);

            // 3. Tự động lưu lịch sử hội thoại vào Database
            var chatMessage = new ChatMessage
            {
                ChatMessageId = Guid.NewGuid(),
                UserId = request.UserId, // Null nếu là khách chưa đăng nhập
                UserMessage = request.Message,
                BotResponse = botReply,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ChatMessages.AddAsync(chatMessage);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 4. Trả về kết quả
            return ApiResponse<ChatResponseDto>.SuccessResult(new ChatResponseDto { Response = botReply }, "Phản hồi thành công.");
        }
    }
}