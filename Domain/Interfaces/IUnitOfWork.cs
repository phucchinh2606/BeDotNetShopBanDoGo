namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        INewsRepository News { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        IProductRepository Products { get; }
        IProductImageRepository ProductImages { get; }
        IReviewRepository Reviews { get; }
        IUserRepository Users { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }

        Task<int> SaveChangesAsync();
    }
}
