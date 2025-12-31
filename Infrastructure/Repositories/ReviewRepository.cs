using EquipmentShop.Core.Entities;
using EquipmentShop.Core.Interfaces;
using EquipmentShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EquipmentShop.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ReviewRepository> _logger;

        public ReviewRepository(AppDbContext context, ILogger<ReviewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Review?> GetReviewAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetProductReviewsAsync(int productId, bool onlyApproved = true)
        {
            var query = _context.Reviews
                .Where(r => r.ProductId == productId);

            if (onlyApproved)
            {
                query = query.Where(r => r.IsApproved);
            }

            return await query
                .OrderByDescending(r => r.IsFeatured)
                .ThenByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetUserReviewsAsync(string userId)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> FindAsync(Expression<Func<Review, bool>> predicate)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            try
            {
                review.CreatedAt = DateTime.UtcNow;
                review.IsApproved = true; // Автоматически одобряем

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                // Обновляем рейтинг товара
                await UpdateProductRatingAsync(review.ProductId);

                return review;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании отзыва");
                throw;
            }
        }

        public async Task UpdateReviewAsync(Review review)
        {
            try
            {
                review.UpdatedAt = DateTime.UtcNow;
                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();

                // Обновляем рейтинг товара
                await UpdateProductRatingAsync(review.ProductId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении отзыва {ReviewId}", review.Id);
                throw;
            }
        }

        public async Task DeleteReviewAsync(int id)
        {
            try
            {
                var review = await GetReviewAsync(id);
                if (review == null)
                {
                    throw new Exception($"Отзыв с ID {id} не найден");
                }

                var productId = review.ProductId;

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                // Обновляем рейтинг товара
                await UpdateProductRatingAsync(productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении отзыва {ReviewId}", id);
                throw;
            }
        }

        public async Task<bool> CanUserReviewProductAsync(string userId, int productId)
        {
            // Проверяем, оставлял ли пользователь уже отзыв на этот товар
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);

            return existingReview == null;
        }

        public async Task<bool> UserHasPurchasedProductAsync(string userId, int productId)
        {
            // В реальном приложении здесь была бы проверка покупок
            // Для демо возвращаем true
            return true;
        }

        public async Task ApproveReviewAsync(int id)
        {
            var review = await GetReviewAsync(id);
            if (review == null)
            {
                throw new Exception($"Отзыв с ID {id} не найден");
            }

            review.IsApproved = true;
            await UpdateReviewAsync(review);
        }

        public async Task RejectReviewAsync(int id, string reason)
        {
            var review = await GetReviewAsync(id);
            if (review == null)
            {
                throw new Exception($"Отзыв с ID {id} не найден");
            }

            review.IsApproved = false;
            review.AdminResponse = $"Отклонено модератором: {reason}";
            await UpdateReviewAsync(review);
        }

        public async Task AddAdminResponseAsync(int reviewId, string response)
        {
            var review = await GetReviewAsync(reviewId);
            if (review == null)
            {
                throw new Exception($"Отзыв с ID {reviewId} не найден");
            }

            review.AdminResponse = response;
            review.AdminResponseDate = DateTime.UtcNow;
            await UpdateReviewAsync(review);
        }

        public async Task<ReviewStats> GetProductReviewStatsAsync(int productId)
        {
            var reviews = await GetProductReviewsAsync(productId, true);

            var stats = new ReviewStats
            {
                TotalReviews = reviews.Count(),
                VerifiedPurchases = reviews.Count(r => r.IsVerifiedPurchase),
                FeaturedReviews = reviews.Count(r => r.IsFeatured),
                WithPros = reviews.Count(r => r.Pros.Any()),
                WithCons = reviews.Count(r => r.Cons.Any())
            };

            if (stats.TotalReviews > 0)
            {
                stats.AverageRating = reviews.Average(r => r.Rating);

                // Распределение по рейтингам
                for (int i = 1; i <= 5; i++)
                {
                    stats.RatingDistribution[i] = reviews.Count(r => r.Rating == i);
                }
            }

            return stats;
        }

        private async Task UpdateProductRatingAsync(int productId)
        {
            var reviews = await GetProductReviewsAsync(productId, true);

            if (reviews.Any())
            {
                var averageRating = reviews.Average(r => r.Rating);
                var reviewsCount = reviews.Count();

                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.Rating = Math.Round(averageRating, 1);
                    product.ReviewsCount = reviewsCount;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<Review> AddAsync(Review review)
        {
            return await CreateReviewAsync(review);
        }

        public async Task<int> CountAsync(Expression<Func<Review, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await _context.Reviews.CountAsync();
            }

            return await _context.Reviews.CountAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Review, bool>> predicate)
        {
            return await _context.Reviews.AnyAsync(predicate);
        }
    }
}
