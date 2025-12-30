using System.ComponentModel.DataAnnotations;

namespace EquipmentShop.Core.Enums
{
    public enum ProductCondition
    {
        [Display(Name = "Новый")]
        New = 1,

        [Display(Name = "Б/У")]
        Used = 2,

        [Display(Name = "Восстановленный")]
        Refurbished = 3,

        [Display(Name = "Демонстрационный")]
        Demo = 4
    }
}
