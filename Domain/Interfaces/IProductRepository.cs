using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByIdWithCategoryAsync(Guid productId);

        Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedProductsAsync(
            string? searchTerm,
            Guid? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            ProductStatus? status,
            string? sortBy,
            bool isDescending,
            int pageNumber,
            int pageSize);
    }
}
