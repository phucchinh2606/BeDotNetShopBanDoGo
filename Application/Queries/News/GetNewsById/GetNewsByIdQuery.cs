using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.News.GetNewsById
{
    public class GetNewsByIdQuery : IRequest<ApiResponse<NewsDto>>
    {
        public Guid NewsId { get; set; }

        public GetNewsByIdQuery(Guid newsId)
        {
            NewsId = newsId;
        }
    }
}
