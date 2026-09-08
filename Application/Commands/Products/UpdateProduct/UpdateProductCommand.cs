using Application.Commons.Models;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.Commands.Products.UpdateProduct
{
    public class UpdateProductCommand : IRequest<ApiResponse<bool>>
    {
        [JsonIgnore] // Id được truyền qua Route
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string Dimensions { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductStatus Status { get; set; }
        public IFormFile? Image { get; set; } // File ảnh mới (nếu muốn thay đổi)
    }
}
