using EquipmentShop.Core.Entities;

namespace EquipmentShop.Core.Entities
{
    public class ShoppingCart
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(30);

        // Навигационные свойства
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        // Вычисляемые свойства
        public decimal Subtotal => Items.Sum(i => i.TotalPrice);
        public int TotalItems => Items.Sum(i => i.Quantity);
        public int UniqueItemsCount => Items.Count;

        public bool IsEmpty => !Items.Any();
        public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;

        // Методы
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void Clear()
        {
            Items.Clear();
            UpdatedAt = DateTime.UtcNow;
        }

        public CartItem? GetItem(int productId)
        {
            return Items.FirstOrDefault(i => i.ProductId == productId);
        }
    }
}
