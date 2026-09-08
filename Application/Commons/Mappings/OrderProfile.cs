using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
        }
    }
}
