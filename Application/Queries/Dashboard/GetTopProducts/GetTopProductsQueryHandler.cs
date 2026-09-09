using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Dashboard.GetTopProducts
{
    public class GetTopProductsQueryHandler : IRequestHandler<GetTopProductsQuery, ApiResponse<IEnumerable<TopProductDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTopProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<TopProductDto>>> Handle(GetTopProductsQuery request, CancellationToken cancellationToken)
        {
            // 1. Lấy tất cả chi tiết đơn hàng (OrderDetail) thuộc về các đơn hàng đã hoàn thành (Delivered)
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var deliveredOrders = orders.Where(o => o.OrderStatus == OrderStatus.Delivered).ToList();

            var deliveredOrderIds = deliveredOrders.Select(o => o.OrderId).ToHashSet();

            var orderDetails = await _unitOfWork.OrderDetails.GetAllAsync();
            var validOrderDetails = orderDetails.Where(od => deliveredOrderIds.Contains(od.OrderId)).ToList();

            // 2. Gom nhóm theo ProductId và tính tổng số lượng bán ra + tổng doanh thu
            var productSales = validOrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantitySold = g.Sum(od => od.Quantity),
                    TotalRevenue = g.Sum(od => od.Quantity * od.UnitPrice)
                })
                .OrderByDescending(x => x.TotalQuantitySold) // Sắp xếp theo số lượng bán nhiều nhất
                .ThenByDescending(x => x.TotalRevenue)
                .Take(request.Limit)
                .ToList();

            // 3. Lấy thông tin chi tiết của Sản phẩm để trả về DTO
            var topProducts = new List<TopProductDto>();
            foreach (var sale in productSales)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(sale.ProductId);
                if (product != null)
                {
                    topProducts.Add(new TopProductDto
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Material = product.Material,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        TotalQuantitySold = sale.TotalQuantitySold,
                        TotalRevenue = sale.TotalRevenue
                    });
                }
            }

            return ApiResponse<IEnumerable<TopProductDto>>.SuccessResult(
                topProducts,
                $"Lấy danh sách Top {topProducts.Count} sản phẩm bán chạy thành công."
            );
        }
    }
}
