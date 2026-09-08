namespace Domain.Entities
{
    public class CartItem
    {
        public Guid CartItemId { get; set; } = Guid.NewGuid();
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}