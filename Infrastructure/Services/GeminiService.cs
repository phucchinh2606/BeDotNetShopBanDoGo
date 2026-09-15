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

        // TỐI ƯU 3: Đưa các model Lite / Flash siêu tốc lên ưu tiên hàng đầu
        private readonly List<string> _models = new List<string>
        {
            "gemini-3.1-flash-lite",
            "gemini-2.5-flash-lite",
            "gemini-2.5-flash",
            "gemini-flash-latest"
        };

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
        }

        public async Task<string> GenerateChatResponseAsync(string userMessage, string contextData, CancellationToken cancellationToken = default)
        {
            var requestPayload = CreateRequestBody(userMessage, contextData);
            var content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

            foreach (var model in _models)
            {
                var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={_apiKey}";
                var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseJson);
                    try
                    {
                        return result.candidates[0].content.parts[0].text;
                    }
                    catch
                    {
                        return "Rất tiếc, tôi chưa thể xử lý câu hỏi này.";
                    }
                }

                if ((int)response.StatusCode == 503)
                {
                    continue; // Chuyển sang model dự phòng nếu bị 503
                }
                break;
            }

            return "Hệ thống AI hiện đang bận, bạn vui lòng thử lại sau nhé!";
        }

        public async IAsyncEnumerable<string> StreamChatResponseAsync(string userMessage, string contextData, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var requestPayload = CreateRequestBody(userMessage, contextData);
            var content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

            // Gọi tới API streamGenerateContent để nhận dữ liệu dạng Server-Sent Events (SSE)
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite:streamGenerateContent?key={_apiKey}&alt=sse";

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint) { Content = content };
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (line != null && line.StartsWith("data: "))
                    {
                        var json = line.Substring(6);
                        dynamic data = JsonConvert.DeserializeObject(json);
                        string chunk = data?.candidates?[0]?.content?.parts?[0]?.text;

                        if (!string.IsNullOrEmpty(chunk))
                        {
                            yield return chunk;
                        }
                    }
                }
            }
        }

        private object CreateRequestBody(string userMessage, string contextData)
        {
            var systemPrompt = $@"Bạn là trợ lý AI tư vấn bán đồ gỗ nội thất mỹ nghệ cao cấp. 
Trả lời ngắn gọn, lịch sự, chính xác dựa trên danh sách sản phẩm bên dưới.
DANH SÁCH SẢN PHẨM:
{contextData}";

            return new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[] { new { text = $"{systemPrompt}\n\nCâu hỏi khách hàng: {userMessage}" } }
                    }
                },
                generationConfig = new
                {
                    maxOutputTokens = 300, // TỐI ƯU 4: Khống chế tối đa 300 tokens câu trả lời
                    temperature = 0.3      // Giảm thời gian suy luận
                }
            };
        }
    }
}