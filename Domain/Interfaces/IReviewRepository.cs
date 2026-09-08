using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<Review?> GetReviewByUserAndProductAsync(Guid userId, Guid productId);
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId);
    }
}
