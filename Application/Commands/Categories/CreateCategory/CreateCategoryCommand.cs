using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.Categories.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Guid>
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; } // Nullable nếu là danh mục cha cao nhất
    }
}
