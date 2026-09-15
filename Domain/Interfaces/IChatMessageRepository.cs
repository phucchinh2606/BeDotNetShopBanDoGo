using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IChatMessageRepository : IGenericRepository<ChatMessage>
    {
        // Có thể bổ sung thêm hàm lấy lịch sử chat theo UserId nếu cần
        Task<IEnumerable<ChatMessage>> GetByUserIdAsync(Guid userId);
    }
}
