using Microsoft.EntityFrameworkCore;
using EquipmentShop.Core.Entities;
using EquipmentShop.Core.Enums;
using System.Text.Json;

namespace EquipmentShop.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Wishlist> Wishlists => Set<Wishlist>();
        public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.Slug).IsUnique();
                entity.HasIndex(p => p.CategoryId);
                entity.HasIndex(p => p.Brand);
                entity.HasIndex(p => p.Price);
                entity.HasIndex(p => p.IsFeatured);
                entity.HasIndex(p => p.CreatedAt);
                entity.HasIndex(p => new { p.IsAvailable, p.StockQuantity });

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Slug)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Description)
                    .HasMaxLength(4000);

                entity.Property(p => p.ShortDescription)
                    .HasMaxLength(500);

                entity.Property(p => p.Price)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(p => p.OldPrice)
                    .HasPrecision(18, 2);

                entity.Property(p => p.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(p => p.Brand)
                    .HasMaxLength(100);

                entity.Property(p => p.Rating)
                    .HasDefaultValue(0.0);

                entity.Property(p => p.StockQuantity)
                    .HasDefaultValue(0);

                entity.Property(p => p.IsAvailable)
                    .HasComputedColumnSql("[StockQuantity] > 0");

                // Конвертация списков в JSON
                entity.Property(p => p.GalleryImages)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new List<string>()
                    );

                entity.Property(p => p.Specifications)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, JsonSerializerOptions.Default) ?? new Dictionary<string, string>()
                    );

                entity.Property(p => p.Tags)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new List<string>()
                    );

                // Отношения
                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.Reviews)
                    .WithOne(r => r.Product)
                    .HasForeignKey(r => r.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.OrderItems)
                    .WithOne(oi => oi.Product)
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Конфигурация Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasIndex(c => c.Slug).IsUnique();
                entity.HasIndex(c => c.ParentCategoryId);
                entity.HasIndex(c => c.DisplayOrder);
                entity.HasIndex(c => c.IsActive);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Slug)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Description)
                    .HasMaxLength(1000);

                entity.Property(c => c.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(c => c.IconClass)
                    .HasMaxLength(50);

                entity.Property(c => c.MetaTitle)
                    .HasMaxLength(200);

                entity.Property(c => c.MetaDescription)
                    .HasMaxLength(500);

                // Иерархия
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Конфигурация Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasIndex(o => o.OrderNumber).IsUnique();
                entity.HasIndex(o => o.CustomerEmail);
                entity.HasIndex(o => o.UserId);
                entity.HasIndex(o => o.Status);
                entity.HasIndex(o => o.PaymentStatus);
                entity.HasIndex(o => o.OrderDate);

                entity.Property(o => o.OrderNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(o => o.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(o => o.CustomerEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(o => o.CustomerPhone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(o => o.ShippingAddress)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(o => o.ShippingCity)
                    .HasMaxLength(100);

                entity.Property(o => o.ShippingRegion)
                    .HasMaxLength(100);

                entity.Property(o => o.ShippingPostalCode)
                    .HasMaxLength(20);

                entity.Property(o => o.ShippingCountry)
                    .HasMaxLength(50);

                entity.Property(o => o.ShippingNotes)
                    .HasMaxLength(1000);

                entity.Property(o => o.TrackingNumber)
                    .HasMaxLength(100);

                entity.Property(o => o.ShippingProvider)
                    .HasMaxLength(100);

                entity.Property(o => o.AdminNotes)
                    .HasMaxLength(2000);

                entity.Property(o => o.CustomerNotes)
                    .HasMaxLength(1000);

                // Конвертация enum
                entity.Property(o => o.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v)
                    );

                entity.Property(o => o.PaymentMethod)
                    .HasConversion(
                        v => v.ToString(),
                        v => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), v)
                    );

                entity.Property(o => o.PaymentStatus)
                    .HasConversion(
                        v => v.ToString(),
                        v => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), v)
                    );

                // Деньги
                entity.Property(o => o.Subtotal)
                    .HasPrecision(18, 2);

                entity.Property(o => o.ShippingCost)
                    .HasPrecision(18, 2);

                entity.Property(o => o.TaxAmount)
                    .HasPrecision(18, 2);

                entity.Property(o => o.DiscountAmount)
                    .HasPrecision(18, 2);

                // Отношения
                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.HasIndex(oi => oi.OrderId);
                entity.HasIndex(oi => oi.ProductId);

                entity.Property(oi => oi.ProductName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(oi => oi.ProductSku)
                    .HasMaxLength(100);

                entity.Property(oi => oi.UnitPrice)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(oi => oi.OriginalPrice)
                    .HasPrecision(18, 2);

                entity.Property(oi => oi.ProductAttributes)
                    .HasMaxLength(1000);
            });

            // Конфигурация ShoppingCart
            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasIndex(c => c.UserId);

                entity.Property(c => c.Id)
                    .HasMaxLength(50);

                entity.Property(c => c.UserId)
                    .HasMaxLength(450);

                // Отношения
                entity.HasMany(c => c.Items)
                    .WithOne(i => i.Cart)
                    .HasForeignKey(i => i.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация CartItem
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.HasIndex(i => i.CartId);
                entity.HasIndex(i => i.ProductId);

                entity.Property(i => i.CartId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(i => i.Price)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(i => i.SelectedAttributes)
                    .HasMaxLength(1000);
            });

            // Конфигурация Review
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasIndex(r => r.ProductId);
                entity.HasIndex(r => r.UserId);
                entity.HasIndex(r => r.Rating);
                entity.HasIndex(r => r.IsApproved);
                entity.HasIndex(r => r.IsFeatured);
                entity.HasIndex(r => r.CreatedAt);

                entity.Property(r => r.UserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(r => r.UserEmail)
                    .HasMaxLength(100);

                entity.Property(r => r.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(r => r.Comment)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(r => r.OrderId)
                    .HasMaxLength(50);

                entity.Property(r => r.AdminResponse)
                    .HasMaxLength(1000);

                // Конвертация списков в JSON
                entity.Property(r => r.Pros)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new List<string>()
                    );

                entity.Property(r => r.Cons)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new List<string>()
                    );

                // Ограничения
                object value = entity.HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5");
            });

            // Конфигурация Wishlist
            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.HasIndex(w => w.UserId).IsUnique();

                entity.Property(w => w.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            // Конфигурация WishlistItem
            modelBuilder.Entity<WishlistItem>(entity =>
            {
                entity.HasKey(wi => wi.Id);
                entity.HasIndex(wi => wi.WishlistId);
                entity.HasIndex(wi => wi.ProductId);

                entity.HasIndex(wi => new { wi.WishlistId, wi.ProductId })
                    .IsUnique();
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Product || e.Entity is Order || e.Entity is Review);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is Product product)
                    {
                        product.UpdatedAt = DateTime.UtcNow;
                    }
                    else if (entry.Entity is Order order)
                    {
                        // Автоматическое обновление дат при изменении статуса
                        if (entry.OriginalValues["Status"] != entry.CurrentValues["Status"])
                        {
                            var newStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), entry.CurrentValues["Status"]?.ToString() ?? "Pending");

                            if (newStatus == OrderStatus.Processing && order.ProcessingDate == null)
                            {
                                order.ProcessingDate = DateTime.UtcNow;
                            }
                            else if (newStatus == OrderStatus.Shipped && order.ShippedDate == null)
                            {
                                order.ShippedDate = DateTime.UtcNow;
                            }
                            else if (newStatus == OrderStatus.Delivered && order.DeliveredDate == null)
                            {
                                order.DeliveredDate = DateTime.UtcNow;
                            }
                            else if (newStatus == OrderStatus.Cancelled && order.CancelledDate == null)
                            {
                                order.CancelledDate = DateTime.UtcNow;
                            }
                        }
                    }
                    else if (entry.Entity is Review review)
                    {
                        review.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
