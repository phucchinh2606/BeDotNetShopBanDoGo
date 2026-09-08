namespace Application.Commons.DTOs
{
    public class ReviewDto
    {
        public Guid ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; } // Hiển thị tên người đánh giá
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
