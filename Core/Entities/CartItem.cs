namespace EquipmentShop.Core.Entities
{
    public class CartItem
    {
        public int Id { get; set; }

        // Связи
        public string CartId { get; set; } = string.Empty;
        public ShoppingCart Cart { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // Количество и цена
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }

        // Дополнительная информация
        public string? SelectedAttributes { get; set; } // JSON с выбранными атрибутами
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Вычисляемые свойства
        public decimal TotalPrice => Price * Quantity;

        // Методы
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }

            Quantity = newQuantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncreaseQuantity(int amount = 1)
        {
            Quantity += amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DecreaseQuantity(int amount = 1)
        {
            if (Quantity - amount < 1)
            {
                throw new InvalidOperationException("Quantity cannot be less than 1");
            }

            Quantity -= amount;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
