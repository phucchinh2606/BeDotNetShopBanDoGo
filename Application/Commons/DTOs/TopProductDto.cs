namespace Application.Commons.DTOs
{
    public class TopProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }  // Tổng số lượng đã bán ra
        public decimal TotalRevenue { get; set; }     // Tổng doanh thu sản phẩm này mang lại
    }
}
