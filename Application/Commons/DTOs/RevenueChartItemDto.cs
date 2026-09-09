namespace Application.Commons.DTOs
{
    public class RevenueChartItemDto
    {
        public string Label { get; set; } = string.Empty; // Nhãn thời gian (VD: "2026-09-09", "09/2026", "2026")
        public decimal Revenue { get; set; }              // Doanh thu trong khoảng thời gian đó
        public int OrderCount { get; set; }               // Số lượng đơn hàng tương ứng
    }
}
