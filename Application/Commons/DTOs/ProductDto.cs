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
        public ProductStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
