using Application.Commons.Models;
using MediatR;

namespace Application.Commands.Products.DeleteProduct
{
    public class DeleteProductCommand : IRequest<ApiResponse<bool>>
    {
        public Guid ProductId { get; set; }

        public DeleteProductCommand(Guid productId)
        {
            ProductId = productId;
        }
    }
}
