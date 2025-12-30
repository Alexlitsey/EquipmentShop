namespace EquipmentShop.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }

        // Связи
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string? UserId { get; set; }
        public string? OrderId { get; set; }

        // Информация о пользователе
        public string UserName { get; set; } = string.Empty;
        public string? UserEmail { get; set; }

        // Содержание отзыва
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5

        // Дополнительная информация
        public List<string> Pros { get; set; } = new();
        public List<string> Cons { get; set; } = new();
        public bool IsVerifiedPurchase { get; set; }
        public bool IsRecommended { get; set; } = true;

        // Модерация
        public bool IsApproved { get; set; } = true;
        public bool IsFeatured { get; set; }
        public string? AdminResponse { get; set; }
        public DateTime? AdminResponseDate { get; set; }

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Вычисляемые свойства
        public string RatingStars
        {
            get
            {
                return string.Join("", Enumerable.Repeat("★", Rating)) +
                       string.Join("", Enumerable.Repeat("☆", 5 - Rating));
            }
        }

        public bool HasResponse => !string.IsNullOrEmpty(AdminResponse);

        // Методы
        public bool CanBeEditedByUser(string userId)
        {
            return UserId == userId && CreatedAt > DateTime.UtcNow.AddDays(-7);
        }
    }
}
