using Datenlotsen.InventoryManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datenlotsen.InventoryManagement.Data.EntityTypeConfigurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasMany(c => c.InventoryItems)
            .WithOne(i => i.Category)
            .HasForeignKey(i => i.CategoryId);
        builder.HasIndex(c => c.Name)
            .IsUnique();
        builder.HasData([
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Beverages - Alcoholic",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Snacks",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Fruits",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Vegetables",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Dairy",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Meat",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Seafood",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Grains",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Condiments",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Baking",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Frozen Foods",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Canned Foods",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Packaged Foods",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        ]);
    }
}