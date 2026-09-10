using Application.Commons.DTOs;
using Application.Commons.Exceptions;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.News.GetNewsById
{
    public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, ApiResponse<NewsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetNewsByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<NewsDto>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var news = await _unitOfWork.News.GetByIdWithAuthorAsync(request.NewsId);

            if (news == null)
            {
                throw new NotFoundException("Bài viết tin tức", request.NewsId);
            }

            var newsDto = _mapper.Map<NewsDto>(news);
            return ApiResponse<NewsDto>.SuccessResult(newsDto, "Lấy thông tin tin tức thành công.");
        }
    }
}
