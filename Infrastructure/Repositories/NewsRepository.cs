using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(ApplicationDbContext context) : base(context) { }

        public async Task<News?> GetByIdWithAuthorAsync(Guid newsId)
        {
            return await _context.News
                .Include(n => n.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NewsId == newsId);
        }

        public async Task<(IEnumerable<News> Items, int TotalCount)> GetPagedNewsAsync(
            string? searchTerm,
            NewsStatus? status,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize)
        {
            var query = _context.News
                .Include(n => n.Author)
                .AsNoTracking()
                .AsQueryable();

            // 1. Filter theo từ khóa (Tiêu đề, Tóm tắt hoặc Nội dung)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(n => n.Title.ToLower().Contains(term)
                                      || n.Summary.ToLower().Contains(term)
                                      || n.Content.ToLower().Contains(term));
            }

            // 2. Filter theo Status (Draft, Published, Archived)
            if (status.HasValue)
            {
                query = query.Where(n => n.Status == status.Value);
            }

            // 3. Sắp xếp
            query = sortBy?.ToLower() switch
            {
                "title" => isDescending ? query.OrderByDescending(n => n.Title) : query.OrderBy(n => n.Title),
                _ => isDescending ? query.OrderByDescending(n => n.CreatedAt) : query.OrderBy(n => n.CreatedAt)
            };

            // 4. Đếm tổng số bản ghi
            var totalCount = await query.CountAsync();

            // 5. Phân trang
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
