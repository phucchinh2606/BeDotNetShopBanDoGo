using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.Dashboard.GetRevenueChart
{
    public class GetRevenueChartQuery : IRequest<ApiResponse<RevenueChartDto>>
    {
        public string Period { get; set; } = "month"; // Mặc định lọc theo tháng: "day", "month", "year"
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
