using Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IGeminiService _geminiService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public ChatHub(IGeminiService geminiService, IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _geminiService = geminiService;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task SendMessageStream(string userMessage)
        {
            // Lấy context từ Cache
            string contextData = await _cache.GetOrCreateAsync("AI_PRODUCT_CONTEXT", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = System.TimeSpan.FromHours(1);
                var products = await _unitOfWork.Products.GetAllAsync();
                var sb = new StringBuilder();
                foreach (var p in products)
                {
                    sb.AppendLine($"- {p.ProductName} | Giá: {p.Price:N0}đ | Kích thước: {p.Dimensions} | Chất liệu: {p.Material}");
                }
                return sb.ToString();
            });

            // Gửi từng chunk từ streaming API của Gemini tới Client qua WebSocket
            await foreach (var chunk in _geminiService.StreamChatResponseAsync(userMessage, contextData))
            {
                await Clients.Caller.SendAsync("ReceiveMessageChunk", chunk);
            }

            await Clients.Caller.SendAsync("ReceiveMessageComplete");
        }
    }
}
