
namespace EquipmentShop.Core.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }

        public InsufficientStockException(int productId, string productName,
            int requestedQuantity, int availableQuantity)
            : base($"Недостаточно товара '{productName}'. Запрошено: {requestedQuantity}, В наличии: {availableQuantity}")
        {
            ProductId = productId;
            ProductName = productName;
            RequestedQuantity = requestedQuantity;
            AvailableQuantity = availableQuantity;
        }
    }
}
