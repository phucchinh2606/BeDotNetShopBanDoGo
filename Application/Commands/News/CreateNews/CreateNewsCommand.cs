using Application.Commons.Models;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.News.CreateNews
{
    public class CreateNewsCommand : IRequest<ApiResponse<Guid>>
    {
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public NewsStatus Status { get; set; } = NewsStatus.Draft;
        public Guid AuthorId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
