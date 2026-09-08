using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.Mappings
{
        public class ProductProfile : Profile
        {
            public ProductProfile()
            {
                CreateMap<Product, ProductDto>()
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : string.Empty));
            }
        }
    
}
