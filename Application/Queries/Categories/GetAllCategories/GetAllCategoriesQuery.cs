using Application.Commons.DTOs;
using MediatR;

namespace Application.Queries.Categories.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
    {
    }
}
