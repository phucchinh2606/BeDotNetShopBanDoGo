using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Products.CreateProduct
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public Guid CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Material { get; set; }
        public string Dimensions { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.InStock;

        // Ảnh chính đại diện cho sản phẩm
        public IFormFile MainImage { get; set; }

        // Danh sách ảnh phụ/ảnh chi tiết
        public List<IFormFile> SubImages { get; set; } = new List<IFormFile>();
    }
}
