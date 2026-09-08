using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.Mappings
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src =>
                    src.Product.ProductImages.FirstOrDefault(img => img.IsPrimary) != null
                        ? src.Product.ProductImages.FirstOrDefault(img => img.IsPrimary)!.ImageUrl
                        : src.Product.ImageUrl));
        }
    }
}
