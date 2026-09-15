using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Interfaces;
using MediatR;
using System.Text;

namespace Application.Commands.Chat
{
    public class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, ApiResponse<ChatResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeminiService _geminiService;

        public SendChatMessageCommandHandler(IUnitOfWork unitOfWork, IGeminiService geminiService)
        {
            _unitOfWork = unitOfWork;
            _geminiService = geminiService;
        }

        public async Task<ApiResponse<ChatResponseDto>> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return ApiResponse<ChatResponseDto>.FailureResult("Nội dung tin nhắn không được để trống.");
            }

            // 1. RAG: Lấy danh sách sản phẩm từ DB làm Context cho Gemini
            var products = await _unitOfWork.Products.GetAllAsync();
            var contextBuilder = new StringBuilder();

            foreach (var p in products)
            {
                contextBuilder.AppendLine($"- Tên: {p.ProductName} | Giá: {p.Price:N0} VNĐ | Chất liệu: {p.Material} | Kích thước: {p.Dimensions} | Mô tả: {p.Description}");
            }

            // 2. Gọi Gemini API
            var botReply = await _geminiService.GenerateChatResponseAsync(request.Message, contextBuilder.ToString());

            // 3. Chuẩn hóa kết quả trả về bằng DTO và ApiResponse
            var responseDto = new ChatResponseDto
            {
                Response = botReply
            };

            return ApiResponse<ChatResponseDto>.SuccessResult(responseDto, "Phản hồi từ AI Chatbot thành công.");
        }
    }
}