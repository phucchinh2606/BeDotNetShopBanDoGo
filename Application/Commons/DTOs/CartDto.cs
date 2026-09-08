namespace Application.Commons.DTOs
{
    public class CartDto
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal GrandTotal => Items.Sum(x => x.TotalPrice);
        public int TotalItems => Items.Sum(x => x.Quantity);
    }
}
