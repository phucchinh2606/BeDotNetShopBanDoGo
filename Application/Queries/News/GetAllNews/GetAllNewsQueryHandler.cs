using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.News.GetAllNews
{
    public class GetAllNewsQueryHandler : IRequestHandler<GetAllNewsQuery, ApiResponse<PagedResult<NewsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllNewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<NewsDto>>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _unitOfWork.News.GetPagedNewsAsync(
                request.SearchTerm,
                request.Status,
                request.SortBy,
                request.IsDescending,
                request.PageNumber,
                request.PageSize);

            var newsDtos = _mapper.Map<IEnumerable<NewsDto>>(items);

            var pagedResult = new PagedResult<NewsDto>(newsDtos, totalCount, request.PageNumber, request.PageSize);

            return ApiResponse<PagedResult<NewsDto>>.SuccessResult(pagedResult, "Lấy danh sách tin tức thành công.");
        }
    }
}
