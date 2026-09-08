using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class OrderDetail
    {
        public Guid OrderDetailId { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
