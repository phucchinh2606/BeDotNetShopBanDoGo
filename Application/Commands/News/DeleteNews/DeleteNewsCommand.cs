using Application.Commons.Models;
using MediatR;

namespace Application.Commands.News.DeleteNews
{
    public class DeleteNewsCommand : IRequest<ApiResponse<bool>>
    {
        public Guid NewsId { get; set; }

        public DeleteNewsCommand(Guid newsId)
        {
            NewsId = newsId;
        }
    }
}
