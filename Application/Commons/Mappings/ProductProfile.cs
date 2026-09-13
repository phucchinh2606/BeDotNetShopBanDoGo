using Application.Commands.Products.CreateProduct;
using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.Mappings
{
        public class ProductProfile : Profile
        {
            public ProductProfile()
            {
            // Bổ sung dòng này để sửa lỗi "Missing type map configuration"
            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore()); // ImageUrls sẽ được gán thủ công sau khi upload Cloudinary

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : string.Empty))
                // Bổ sung Map danh sách ảnh phụ (lấy tất cả URL ảnh không phải ảnh chính hoặc lấy toàn bộ ProductImages):
                .ForMember(dest => dest.SubImageUrls, opt => opt.MapFrom(src =>
                    src.ProductImages != null
                        ? src.ProductImages.Where(img => !img.IsPrimary).Select(img => img.ImageUrl).ToList()
                        : new List<string>()));
        }
        }
    
}
