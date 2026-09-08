using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICategoryRepository Categories { get; }
        public INewsRepository News { get; }
        public IOrderRepository Orders { get; }
        public IOrderDetailRepository OrderDetails { get; }
        public IProductRepository Products { get; }
        public IProductImageRepository ProductImages { get; }
        public IReviewRepository Reviews { get; }
        public IUserRepository Users { get; }
        public ICartRepository Carts { get; }
        public ICartItemRepository CartItems { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            ICategoryRepository categoryRepository,
            INewsRepository newsRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            IReviewRepository reviewRepository,
            IUserRepository userRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository)
        {
            _context = context;
            Categories = categoryRepository;
            News = newsRepository;
            Orders = orderRepository;
            OrderDetails = orderDetailRepository;
            Products = productRepository;
            ProductImages = productImageRepository;
            Reviews = reviewRepository;
            Users = userRepository;
            Carts = cartRepository;
            CartItems = cartItemRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
