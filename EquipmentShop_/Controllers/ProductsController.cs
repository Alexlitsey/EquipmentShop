using Microsoft.AspNetCore.Mvc;
using EquipmentShop.Core.Interfaces;

namespace EquipmentShop.Controllers;

public class ProductsController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductsController(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index(string? search = null)
    {
        ViewData["Title"] = "Каталог товаров";

        var products = await _productRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(search))
        {
            products = products.Where(p =>
                p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Brand.Contains(search, StringComparison.OrdinalIgnoreCase));
            ViewData["Search"] = search;
        }

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        ViewData["Title"] = product.Name;
        return View(product);
    }
}