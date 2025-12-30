using System.ComponentModel.DataAnnotations;

namespace EquipmentShop.Core.Enums
{
    public enum ShippingMethod
    {
        [Display(Name = "Самовывоз")]
        Pickup = 1,

        [Display(Name = "Курьерская доставка")]
        Courier = 2,

        [Display(Name = "Почта")]
        Postal = 3,

        [Display(Name = "Транспортная компания")]
        TransportCompany = 4
    }
}
