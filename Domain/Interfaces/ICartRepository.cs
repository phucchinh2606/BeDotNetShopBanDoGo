using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(Guid userId);
        Task<Cart> GetOrCreateCartAsync(Guid userId);
    }
}
