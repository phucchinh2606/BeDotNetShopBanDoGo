using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems));
        }
    }
}
