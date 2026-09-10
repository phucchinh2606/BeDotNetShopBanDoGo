using Application.Commons.Exceptions;
using Application.Commons.Models;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Products.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra sản phẩm có tồn tại hay không
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException("Sản phẩm", request.ProductId);
            }

            // 2. Thực hiện xóa sản phẩm
            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Xóa sản phẩm thành công.");
        }
    }
}
