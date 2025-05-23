using Datenlotsen.InventoryManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datenlotsen.InventoryManagement.Data.EntityTypeConfigurations;

public class InventoryItemEntityTypeConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(i => i.StockQuantity)
            .IsRequired();

        builder
            .HasOne(i => i.Category)
            .WithMany(c => c.InventoryItems)
            .HasForeignKey(i => i.CategoryId);

        builder
            .HasIndex(i => i.Name)
            .IsUnique();
        builder.Navigation(i => i.Category)
            .AutoInclude();
    }
}