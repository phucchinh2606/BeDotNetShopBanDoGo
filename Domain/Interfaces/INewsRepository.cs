using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface INewsRepository : IGenericRepository<News>
    {
        Task<News?> GetByIdWithAuthorAsync(Guid newsId);

        Task<(IEnumerable<News> Items, int TotalCount)> GetPagedNewsAsync(
            string? searchTerm,
            NewsStatus? status,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize);
    }
}
