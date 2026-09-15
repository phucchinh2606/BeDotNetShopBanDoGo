namespace Domain.Interfaces
{
    public interface IGeminiService
    {
        // Trả về full chuỗi response (dành cho API REST)
        Task<string> GenerateChatResponseAsync(string userMessage, string contextData, CancellationToken cancellationToken = default);

        // Trả về luồng dữ liệu từng từ (dành cho SignalR Streaming)
        IAsyncEnumerable<string> StreamChatResponseAsync(string userMessage, string contextData, CancellationToken cancellationToken = default);
    }
}
