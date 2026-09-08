namespace Application.Commons.DTOs
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
        public List<CategoryDto> SubCategories { get; set; } = new List<CategoryDto>();
    }
}
