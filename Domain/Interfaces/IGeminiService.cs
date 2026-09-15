namespace Domain.Interfaces
{
    public interface IGeminiService
    {
        Task<string> GenerateChatResponseAsync(string userMessage, string contextData);
    }
}
