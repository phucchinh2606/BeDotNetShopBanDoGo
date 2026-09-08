using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.Mappings
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.FullName : string.Empty));
        }
    }
}
