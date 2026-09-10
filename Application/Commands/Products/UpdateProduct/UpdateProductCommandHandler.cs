using Application.Commons.Exceptions;
using Application.Commons.Models;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Products.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra sản phẩm có tồn tại hay không
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException("Sản phẩm", request.ProductId);
            }

            // 2. Kiểm tra Danh mục có tồn tại hay không
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new NotFoundException("Danh mục sản phẩm", request.CategoryId);
            }

            // 3. Xử lý ảnh mới (nếu có upload)
            if (request.Image != null && request.Image.Length > 0)
            {
                // string newImageUrl = await _fileService.UploadFileAsync(request.Image);
                // product.ImageUrl = newImageUrl;
            }

            // 4. Cập nhật các thông tin cơ bản
            product.CategoryId = request.CategoryId;
            product.ProductName = request.ProductName;
            product.Material = request.Material;
            product.Dimensions = request.Dimensions;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.Status = request.Status;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Cập nhật thông tin sản phẩm thành công.");
        }
    }
}
