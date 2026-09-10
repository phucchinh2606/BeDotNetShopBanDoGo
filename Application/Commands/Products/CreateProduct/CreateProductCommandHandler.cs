using Application.Commons.Exceptions;
using Application.Commons.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Products.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra Category có tồn tại hay không
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new NotFoundException("Danh mục sản phẩm", request.CategoryId);
            }

            // 2. Map request command sang Entity Product
            var product = _mapper.Map<Product>(request);

            // 3. Upload ảnh chính (MainImage) lên Cloudinary
            if (request.MainImage != null)
            {
                string mainImageUrl = await _photoService.UploadPhotoAsync(request.MainImage, "products");
                product.ImageUrl = mainImageUrl;

                // Thêm vào bảng ProductImage làm ảnh đại diện chính (IsPrimary = true)
                product.ProductImages.Add(new ProductImage
                {
                    ProductId = product.ProductId,
                    ImageUrl = mainImageUrl,
                    IsPrimary = true
                });
            }

            // 4. Upload các ảnh phụ (SubImages) lên Cloudinary
            if (request.SubImages != null && request.SubImages.Count > 0)
            {
                foreach (var file in request.SubImages)
                {
                    string subImageUrl = await _photoService.UploadPhotoAsync(file, "products");
                    product.ProductImages.Add(new ProductImage
                    {
                        ProductId = product.ProductId,
                        ImageUrl = subImageUrl,
                        IsPrimary = false
                    });
                }
            }

            // 5. Lưu thông tin Sản phẩm và danh sách Ảnh vào Database
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return product.ProductId;
        }
    }
}
