using MediatR;

namespace Application.Commands.Categories.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
    }
}
