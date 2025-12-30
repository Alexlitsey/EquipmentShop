using System.ComponentModel.DataAnnotations;

namespace EquipmentShop.Core.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "Ожидает оплаты")]
        Pending = 1,

        [Display(Name = "Оплачен")]
        Paid = 2,

        [Display(Name = "Ошибка оплаты")]
        Failed = 3,

        [Display(Name = "Возвращен")]
        Refunded = 4,

        [Display(Name = "Частично возвращен")]
        PartiallyRefunded = 5
    }
}
