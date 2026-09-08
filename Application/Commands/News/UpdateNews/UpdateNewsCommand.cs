using Application.Commons.Models;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.Commands.News.UpdateNews
{
    public class UpdateNewsCommand : IRequest<ApiResponse<bool>>
    {
        [JsonIgnore] // Lấy ID từ Route URL
        public Guid NewsId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public NewsStatus Status { get; set; }
        public Guid AuthorId { get; set; }
        public IFormFile? Image { get; set; } // File ảnh mới (nếu muốn thay thế)
    }
}
