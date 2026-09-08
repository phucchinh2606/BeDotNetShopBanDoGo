using MediatR;

namespace Application.Commands.Categories.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public Guid CategoryId { get; set; }

        public DeleteCategoryCommand(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
