namespace Application.Commons.DTOs
{
    public class RevenueChartDto
    {
        public string PeriodType { get; set; } = "month"; // "day", "month", hoặc "year"
        public decimal TotalRevenue { get; set; }         // Tổng doanh thu toàn bộ khoảng chọn
        public List<RevenueChartItemDto> DataItems { get; set; } = new List<RevenueChartItemDto>();
    }
}
