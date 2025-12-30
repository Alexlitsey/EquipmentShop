
using System.ComponentModel.DataAnnotations;

namespace EquipmentShop.Core.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? UserAvatar { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public List<string> Pros { get; set; } = new();
        public List<string> Cons { get; set; } = new();
        public bool IsVerifiedPurchase { get; set; }
        public bool IsRecommended { get; set; }
        public bool IsFeatured { get; set; }
        public string? AdminResponse { get; set; }
        public DateTime? AdminResponseDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Вычисляемые свойства
        public string RatingStars => new string('★', Rating) + new string('☆', 5 - Rating);
        public bool HasResponse => !string.IsNullOrEmpty(AdminResponse);
        public string TimeAgo => GetTimeAgo();
        public string ShortComment => Comment.Length > 150 ? Comment.Substring(0, 150) + "..." : Comment;

        private string GetTimeAgo()
        {
            var timeSpan = DateTime.UtcNow - CreatedAt;

            if (timeSpan.TotalDays > 365)
            {
                var years = (int)(timeSpan.TotalDays / 365);
                return $"{years} {GetRussianWord(years, "год", "года", "лет")} назад";
            }

            if (timeSpan.TotalDays > 30)
            {
                var months = (int)(timeSpan.TotalDays / 30);
                return $"{months} {GetRussianWord(months, "месяц", "месяца", "месяцев")} назад";
            }

            if (timeSpan.TotalDays >= 1)
            {
                var days = (int)timeSpan.TotalDays;
                return $"{days} {GetRussianWord(days, "день", "дня", "дней")} назад";
            }

            if (timeSpan.TotalHours >= 1)
            {
                var hours = (int)timeSpan.TotalHours;
                return $"{hours} {GetRussianWord(hours, "час", "часа", "часов")} назад";
            }

            if (timeSpan.TotalMinutes >= 1)
            {
                var minutes = (int)timeSpan.TotalMinutes;
                return $"{minutes} {GetRussianWord(minutes, "минуту", "минуты", "минут")} назад";
            }

            return "только что";
        }

        private string GetRussianWord(int number, string form1, string form2, string form5)
        {
            number %= 100;
            if (number >= 11 && number <= 19)
                return form5;

            number %= 10;
            return number switch
            {
                1 => form1,
                2 or 3 or 4 => form2,
                _ => form5
            };
        }
    }

    public class CreateReviewViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(100, ErrorMessage = "Заголовок должен быть от {2} до {1} символов", MinimumLength = 5)]
        [Display(Name = "Заголовок отзыва")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Текст отзыва обязателен")]
        [StringLength(1000, ErrorMessage = "Отзыв должен быть от {2} до {1} символов", MinimumLength = 10)]
        [Display(Name = "Текст отзыва")]
        public string Comment { get; set; } = string.Empty;

        [Required(ErrorMessage = "Оценка обязательна")]
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        [Display(Name = "Оценка")]
        public int Rating { get; set; } = 5;

        [Display(Name = "Достоинства (каждое с новой строки)")]
        public string? ProsText { get; set; }

        [Display(Name = "Недостатки (каждое с новой строки)")]
        public string? ConsText { get; set; }

        [Display(Name = "Рекомендую этот товар")]
        public bool IsRecommended { get; set; } = true;

        public List<string> GetProsList() =>
            ProsText?.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList()
            ?? new List<string>();

        public List<string> GetConsList() =>
            ConsText?.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList()
            ?? new List<string>();
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
        public int FeaturedReviews { get; set; }
        public int WithPros { get; set; }
        public int WithCons { get; set; }

        // Вычисляемые свойства
        public int OneStarPercent => TotalReviews > 0 ? (RatingDistribution[1] * 100 / TotalReviews) : 0;
        public int TwoStarsPercent => TotalReviews > 0 ? (RatingDistribution[2] * 100 / TotalReviews) : 0;
        public int ThreeStarsPercent => TotalReviews > 0 ? (RatingDistribution[3] * 100 / TotalReviews) : 0;
        public int FourStarsPercent => TotalReviews > 0 ? (RatingDistribution[4] * 100 / TotalReviews) : 0;
        public int FiveStarsPercent => TotalReviews > 0 ? (RatingDistribution[5] * 100 / TotalReviews) : 0;

        public string AverageRatingFormatted => AverageRating.ToString("F1");
        public bool HasReviews => TotalReviews > 0;
    }

    public class ReviewFilter
    {
        public int? Rating { get; set; }
        public bool? VerifiedOnly { get; set; }
        public bool? WithPros { get; set; }
        public bool? WithCons { get; set; }
        public string? SortBy { get; set; } = "recent"; // recent, helpful, highest, lowest
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
