using Domain.Enums;

namespace Application.Commons.DTOs
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string Material { get; set; }
        public string Dimensions { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }

        // Thêm danh sách ảnh phụ ở đây:
        public List<string> SubImageUrls { get; set; } = new List<string>();
        public ProductStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
