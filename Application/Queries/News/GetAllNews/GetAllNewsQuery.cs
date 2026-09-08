using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Enums;
using MediatR;

namespace Application.Queries.News.GetAllNews
{
    public class GetAllNewsQuery : IRequest<ApiResponse<PagedResult<NewsDto>>>
    {
        public string? SearchTerm { get; set; }
        public NewsStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
