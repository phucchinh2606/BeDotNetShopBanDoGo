using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ProductImage
    {
        public Guid ProductImageId { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; } = false;
        public Product Product { get; set; }
    }
}
