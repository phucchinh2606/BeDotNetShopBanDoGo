using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
        }

        public async Task<string> GenerateChatResponseAsync(string userMessage, string contextData)
        {
            // Đã cập nhật mô hình mới nhất theo yêu cầu từ Google Gemini API
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.6-flash:generateContent?key={_apiKey}";

            var systemPrompt = $@"Bạn là trợ lý AI chuyên tư vấn bán đồ gỗ nội thất mỹ nghệ cao cấp. 
Hãy trả lời thân thiện, lịch sự, tư vấn chính xác dựa trên danh sách sản phẩm hiện có dưới đây.
Nếu khách hàng hỏi sản phẩm không có trong danh sách, hãy khéo léo thông báo và gợi ý sản phẩm gần nhất.

DANH SÁCH SẢN PHẨM HIỆN CÓ:
{contextData}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = $"{systemPrompt}\n\nCâu hỏi của khách hàng: {userMessage}" }
                        }
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"=== GEMINI ERROR RESPONSE ({response.StatusCode}) ===");
                Console.WriteLine(errorResponse);
                Console.WriteLine("===============================================");

                return $"Rất tiếc, hệ thống tư vấn AI đang bận. Lỗi chi tiết: {errorResponse}";
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseJson);

            try
            {
                string botReply = result.candidates[0].content.parts[0].text;
                return botReply;
            }
            catch
            {
                return "Xin lỗi, tôi chưa thể xử lý câu hỏi này lúc này.";
            }
        }
    }
}