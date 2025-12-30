

namespace EquipmentShop.Core.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconClass { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public int ProductCount { get; set; }
        public int DisplayOrder { get; set; }
        public bool ShowInMenu { get; set; }
        public IEnumerable<CategoryViewModel> SubCategories { get; set; } = new List<CategoryViewModel>();

        public string FullPath { get; set; } = string.Empty;
        public bool HasSubCategories => SubCategories.Any();
    }

    public class CategoryTreeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int Level { get; set; }
        public List<CategoryTreeViewModel> Children { get; set; } = new();
    }
}
