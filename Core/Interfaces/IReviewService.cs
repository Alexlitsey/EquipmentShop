using EquipmentShop.Core.Entities;

namespace EquipmentShop.Core.Interfaces
{
    public interface IReviewService
    {
        Task<Review?> GetReviewAsync(int id);
        Task<IEnumerable<Review>> GetProductReviewsAsync(int productId, bool onlyApproved = true);
        Task<IEnumerable<Review>> GetUserReviewsAsync(string userId);
        Task<Review> CreateReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
        Task<bool> CanUserReviewProductAsync(string userId, int productId);
        Task<bool> UserHasPurchasedProductAsync(string userId, int productId);
        Task ApproveReviewAsync(int id);
        Task RejectReviewAsync(int id, string reason);
        Task AddAdminResponseAsync(int reviewId, string response);
        Task<ReviewStats> GetProductReviewStatsAsync(int productId);
    }

    public class ReviewStats
    {
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new()
    {
        { 1, 0 },
        { 2, 0 },
        { 3, 0 },
        { 4, 0 },
        { 5, 0 }
    };
        public int VerifiedPurchases { get; set; }
        public int WithImages { get; set; }
        public int FeaturedReviews { get; set; }
        public int WithPros { get; set; }
        public int WithCons { get; set; }
    }
}
