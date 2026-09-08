using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }

        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
