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

            // 1. Quản lý SessionId: Sử dụng SessionId gửi lên hoặc sinh mới nếu chưa có
            string activeSessionId = string.IsNullOrWhiteSpace(request.SessionId)
                ? Guid.NewGuid().ToString()
                : request.SessionId;

            // 2. Lấy Context danh sách sản phẩm từ MemoryCache
            string productContext = await _cache.GetOrCreateAsync("AI_PRODUCT_CONTEXT", async entry =>
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

            // 3. TỐI ƯU NGỮ CẢNH: Lấy 3 tin nhắn gần nhất theo SessionId để giữ mạch trò chuyện
            var recentHistory = (await _unitOfWork.ChatMessages.GetByUserIdAsync(request.UserId ?? Guid.Empty)) // Hoặc Query theo SessionId
                ?.Where(m => m.SessionId == activeSessionId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(3)
                .OrderBy(m => m.CreatedAt)
                .ToList();

            var conversationHistoryBuilder = new StringBuilder();
            if (recentHistory != null && recentHistory.Any())
            {
                conversationHistoryBuilder.AppendLine("LỊCH SỬ TRÒ CHUYỆN GẦN ĐÂY:");
                foreach (var history in recentHistory)
                {
                    conversationHistoryBuilder.AppendLine($"Khách: {history.UserMessage}");
                    conversationHistoryBuilder.AppendLine($"AI: {history.BotResponse}");
                }
            }

            string fullContext = $"{productContext}\n\n{conversationHistoryBuilder}";

            // 4. Gọi Gemini Service
            var botReply = await _geminiService.GenerateChatResponseAsync(request.Message, fullContext, cancellationToken);

            // 5. Lưu lịch sử hội thoại vào Database cùng với SessionId
            var chatMessage = new ChatMessage
            {
                ChatMessageId = Guid.NewGuid(),
                SessionId = activeSessionId,
                UserId = request.UserId,
                UserMessage = request.Message,
                BotResponse = botReply,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ChatMessages.AddAsync(chatMessage);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6. Trả về kết quả kèm SessionId cho Client
            return ApiResponse<ChatResponseDto>.SuccessResult(new ChatResponseDto
            {
                SessionId = activeSessionId,
                Response = botReply
            }, "Phản hồi thành công.");
        }
    }
}