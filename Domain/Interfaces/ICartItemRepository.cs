using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId);
        Task ClearCartItemsAsync(Guid cartId);
    }
}
