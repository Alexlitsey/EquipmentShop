
using EquipmentShop.Core.Enums;

namespace EquipmentShop.Core.ViewModels
{
    public class CartViewModel
    {
        public string CartId { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total => Subtotal + ShippingCost + TaxAmount - DiscountAmount;
        public int TotalItems => Items.Sum(i => i.Quantity);
        public bool IsEmpty => !Items.Any();

        public ShippingMethod ShippingMethod { get; set; } = ShippingMethod.Courier;
        public bool IsShippingRequired { get; set; } = true;

        public bool HasDiscount => DiscountAmount > 0;
    }

    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSlug { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
        public int MaxQuantity { get; set; } = 10;
        public bool IsAvailable { get; set; }
        public string? SelectedAttributes { get; set; }

        public bool CanIncrease => Quantity < MaxQuantity;
        public bool CanDecrease => Quantity > 1;
    }

    public class CartSummaryViewModel
    {
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
        public string FormattedTotal => Total.ToString("C0");
        public bool IsEmpty => ItemCount == 0;
    }
}
