using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.Mappings
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src =>
                    src.Product != null ? src.Product.ProductName : string.Empty));
        }
    }
}
