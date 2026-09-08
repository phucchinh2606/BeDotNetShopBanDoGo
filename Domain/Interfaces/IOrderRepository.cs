using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order> {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
    }
}
