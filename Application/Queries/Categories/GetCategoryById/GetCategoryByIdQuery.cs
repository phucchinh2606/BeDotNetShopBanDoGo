using Application.Commons.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.Categories.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<CategoryDto>
    {
        public Guid CategoryId { get; set; }

        public GetCategoryByIdQuery(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
