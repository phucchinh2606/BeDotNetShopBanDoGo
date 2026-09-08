using Domain.Enums;

namespace Domain.Entities
{
    public class News
    {
        public Guid NewsId { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Slug { get; set; } // Chuỗi URL thân thiện (vd: y-nghia-phong-thuy-tuong-go)
        public string Summary { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public NewsStatus Status { get; set; } = NewsStatus.Draft;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Người viết bài (Admin / Author)
        public Guid AuthorId { get; set; }
        public User Author { get; set; }
    }
}
