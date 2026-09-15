using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .AsNoTracking() // Tối ưu hiệu năng đọc
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<(List<Order> Items, int TotalCount)> GetAdminOrdersAsync(
    int pageNumber,
    int pageSize,
    OrderStatus? orderStatus,
    PaymentStatus? paymentStatus,
    string? searchTerm)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .AsQueryable();

            // 1. Lọc theo trạng thái đơn hàng
            if (orderStatus.HasValue)
            {
                query = query.Where(o => o.OrderStatus == orderStatus.Value);
            }

            // 2. Lọc theo trạng thái thanh toán
            if (paymentStatus.HasValue)
            {
                query = query.Where(o => o.PaymentStatus == paymentStatus.Value);
            }

            // 3. Tìm kiếm theo Tên, SĐT, hoặc OrderCode / OrderId
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();

                // Kiểm tra xem người dùng có gõ chữ số (mã OrderCode) hay không
                bool isNumber = long.TryParse(term, out long searchOrderCode);

                query = query.Where(o =>
                    o.User.FullName.ToLower().Contains(term) ||
                    o.User.PhoneNumber.Contains(term) ||
                    (isNumber && o.OrderCode == searchOrderCode) || // Tim kiem theo OrderCode
                    o.OrderId.ToString().ToLower().Contains(term)
                );
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Order?> GetOrderByIdWithDetailsAsync(Guid orderId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
        public async Task<Order?> GetOrderByIdForUpdateAsync(Guid orderId)
        {
            return await _context.Orders
                // Không có AsNoTracking() để EF Core theo dõi phục vụ update
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order?> GetByOrderCodeAsync(long orderCode)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode);
        }
    }
}
