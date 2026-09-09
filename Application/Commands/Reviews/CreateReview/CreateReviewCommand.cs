using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.Commands.Reviews.CreateReview
{
    public class CreateReviewCommand : IRequest<ApiResponse<ReviewDto>>
    {
        public Guid ProductId { get; set; }
        public int Rating { get; set; } // từ 1 đến 5 sao
        public string Comment { get; set; } = string.Empty;
        public List<IFormFile>? Images { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
