using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Product?> GetByIdWithCategoryAsync(Guid productId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedProductsAsync(
            string? searchTerm,
            Guid? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            ProductStatus? status,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(term)
                                      || p.Material.ToLower().Contains(term)
                                      || p.Description.ToLower().Contains(term));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            query = sortBy?.ToLower() switch
            {
                "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "name" => isDescending ? query.OrderByDescending(p => p.ProductName) : query.OrderBy(p => p.ProductName),
                _ => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
