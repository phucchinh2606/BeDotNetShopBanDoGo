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
        public IChatMessageRepository ChatMessages { get; }

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
            ICartItemRepository cartItemRepository,
            IChatMessageRepository chatMessages)
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
            ChatMessages = chatMessages;
        }

        // Đã cập nhật: Bổ sung tham số cancellationToken (mặc định default)
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
