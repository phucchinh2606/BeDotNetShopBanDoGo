using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public Guid CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Material { get; set; }
        public string Dimensions { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.InStock;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public Category Category { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
