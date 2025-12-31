using EquipmentShop.Core.Interfaces;
using EquipmentShop.Infrastructure.Data;
using EquipmentShop.Infrastructure.Repositories;
using EquipmentShop.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EquipmentShop.Infrastructure.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IReviewService, ReviewRepository>();

            // Services
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            // ApplicationService - ЗАКОММЕНТИРУЙТЕ, если не используете
            // services.AddScoped<IApplicationService, ApplicationService>();

            // OrderService как декоратор над IOrderRepository
            services.AddScoped<IOrderRepository>(sp =>
            {
                var orderRepository = sp.GetRequiredService<OrderRepository>();
                var productRepository = sp.GetRequiredService<IProductRepository>();
                var cartService = sp.GetRequiredService<IShoppingCartService>();
                var logger = sp.GetRequiredService<ILogger<OrderService>>();

                return new OrderService(orderRepository, productRepository, cartService, logger);
            });

            return services;
        }
    }
}
