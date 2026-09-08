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

        public async Task<(IEnumerable<Order> Orders, int TotalCount)> GetAdminOrdersAsync(
            int pageNumber,
            int pageSize,
            OrderStatus? orderStatus,
            PaymentStatus? paymentStatus,
            string? searchTerm)
        {
            var query = _context.Orders
                .AsNoTracking() // Tối ưu hiệu năng đọc
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
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

            // 3. Tìm kiếm theo Từ khóa (Chuẩn hóa cho PostgreSQL)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.Trim();
                query = query.Where(o => EF.Functions.ILike(o.ShippingAddress, $"%{search}%")
                                      || EF.Functions.ILike(o.OrderId.ToString(), $"%{search}%"));
            }

            // 4. Đếm tổng số bản ghi
            int totalCount = await query.CountAsync();

            // 5. Phân trang
            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalCount);
        }

        public async Task<Order?> GetOrderByIdWithDetailsAsync(Guid orderId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
    }
}
