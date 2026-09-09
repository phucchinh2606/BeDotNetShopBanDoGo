using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Dashboard.GetRevenueChart
{
    public class GetRevenueChartQueryHandler : IRequestHandler<GetRevenueChartQuery, ApiResponse<RevenueChartDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRevenueChartQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<RevenueChartDto>> Handle(GetRevenueChartQuery request, CancellationToken cancellationToken)
        {
            var period = request.Period.ToLower();

            // Lấy các đơn hàng đã hoàn thành (Delivered)
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var deliveredOrders = orders
                .Where(o => o.OrderStatus == OrderStatus.Delivered)
                .AsQueryable();

            // Áp dụng bộ lọc thời gian nếu truyền vào
            if (request.FromDate.HasValue)
            {
                deliveredOrders = deliveredOrders.Where(o => o.OrderDate >= request.FromDate.Value);
            }
            if (request.ToDate.HasValue)
            {
                deliveredOrders = deliveredOrders.Where(o => o.OrderDate <= request.ToDate.Value);
            }

            var orderList = deliveredOrders.ToList();
            var chartItems = new List<RevenueChartItemDto>();

            // Gom nhóm theo khoảng thời gian yêu cầu
            switch (period)
            {
                case "day":
                    chartItems = orderList
                        .GroupBy(o => o.OrderDate.Date)
                        .OrderBy(g => g.Key)
                        .Select(g => new RevenueChartItemDto
                        {
                            Label = g.Key.ToString("yyyy-MM-dd"),
                            Revenue = g.Sum(o => o.TotalAmount),
                            OrderCount = g.Count()
                        })
                        .ToList();
                    break;

                case "year":
                    chartItems = orderList
                        .GroupBy(o => o.OrderDate.Year)
                        .OrderBy(g => g.Key)
                        .Select(g => new RevenueChartItemDto
                        {
                            Label = g.Key.ToString(),
                            Revenue = g.Sum(o => o.TotalAmount),
                            OrderCount = g.Count()
                        })
                        .ToList();
                    break;

                case "month":
                default:
                    period = "month";
                    chartItems = orderList
                        .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                        .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                        .Select(g => new RevenueChartItemDto
                        {
                            Label = $"{g.Key.Month:D2}/{g.Key.Year}",
                            Revenue = g.Sum(o => o.TotalAmount),
                            OrderCount = g.Count()
                        })
                        .ToList();
                    break;
            }

            var result = new RevenueChartDto
            {
                PeriodType = period,
                TotalRevenue = chartItems.Sum(x => x.Revenue),
                DataItems = chartItems
            };

            return ApiResponse<RevenueChartDto>.SuccessResult(result, "Lấy dữ liệu biểu đồ doanh thu thành công.");
        }
    }
}
