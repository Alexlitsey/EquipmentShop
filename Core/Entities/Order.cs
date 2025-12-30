

using EquipmentShop.Core.Enums;

namespace EquipmentShop.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = GenerateOrderNumber();

        // Информация о клиенте
        public string? UserId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;

        // Адрес доставки
        public string ShippingAddress { get; set; } = string.Empty;
        public string? ShippingCity { get; set; }
        public string? ShippingRegion { get; set; }
        public string? ShippingPostalCode { get; set; }
        public string? ShippingCountry { get; set; } = "Беларусь";
        public string? ShippingNotes { get; set; }

        // Статусы
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Card;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // Стоимость
        public decimal Subtotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total => Subtotal + ShippingCost + TaxAmount - DiscountAmount;

        // Информация о доставке
        public string? TrackingNumber { get; set; }
        public string? ShippingProvider { get; set; }

        // Даты
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? PaymentDate { get; set; }
        public DateTime? ProcessingDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public DateTime? CancelledDate { get; set; }

        // Дополнительная информация
        public string? AdminNotes { get; set; }
        public string? CustomerNotes { get; set; }

        // Навигационные свойства
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        private static string GenerateOrderNumber()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var randomPart = new Random().Next(10000, 99999);
            return $"DS{datePart}{randomPart}";
        }

        // Методы
        public bool CanBeCancelled()
        {
            return Status == OrderStatus.Pending || Status == OrderStatus.Processing;
        }

        public bool IsPaid => PaymentStatus == PaymentStatus.Paid;
        public bool RequiresPayment => PaymentStatus == PaymentStatus.Pending;
    }

}
