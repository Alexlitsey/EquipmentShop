
using EquipmentShop.Core.Entities;

namespace EquipmentShop.Core.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetBySlugAsync(string slug);
        Task<IEnumerable<Category>> GetMainCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
        Task<IEnumerable<Category>> GetMenuCategoriesAsync();
        Task<IEnumerable<Category>> GetHierarchyAsync();
        Task<bool> HasProductsAsync(int categoryId);
    }
}
