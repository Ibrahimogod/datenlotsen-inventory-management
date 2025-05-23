using Microsoft.EntityFrameworkCore;
using Datenlotsen.InventoryManagement.Data.Models;
using Datenlotsen.InventoryManagement.Shared.Data;

namespace Datenlotsen.InventoryManagement.Data.Tests;

public class RepositoryTests
{
    private readonly string _databaseName;

    public RepositoryTests()
    {
        _databaseName = Guid.NewGuid().ToString();
    }
    
    private InventoryDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options;
        return new InventoryDbContext(options);
    }

    [Fact]
    public async Task AddAsync_AddsEntity()
    {
        await using var ctx = CreateDbContext();
        var repo = new Repository<InventoryDbContext, InventoryItem, Guid>(ctx);
        var item = new InventoryItem { Name = "item", StockQuantity = 1, CategoryId = Guid.NewGuid() };
        var result = await repo.AddAsync(item, CancellationToken.None);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(item.Name, result.Name);
        Assert.True(ctx.InventoryItems.Any());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAll()
    {
        await using var ctx = CreateDbContext();
        var category = new Category { Name = "cat1" };
        ctx.Categories.Add(category);
        await ctx.SaveChangesAsync();
        ctx.InventoryItems.Add(new InventoryItem { Name = "item1", StockQuantity = 1, CategoryId = category.Id });
        ctx.InventoryItems.Add(new InventoryItem { Name = "item2", StockQuantity = 2, CategoryId = category.Id });
        await ctx.SaveChangesAsync();
        var repo = new Repository<InventoryDbContext, InventoryItem, Guid>(ctx);
        var all = await repo.GetAllAsync(i => i, null, true, null, null, null, CancellationToken.None);
        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task GetAllAsync_WithPredicateAndSorting()
    {
        await using var ctx = CreateDbContext();
        var category = new Category { Name = "cat1" };
        ctx.Categories.Add(category);
        await ctx.SaveChangesAsync();
        ctx.InventoryItems.Add(new InventoryItem { Name = "a", StockQuantity = 1, CategoryId = category.Id });
        ctx.InventoryItems.Add(new InventoryItem { Name = "b", StockQuantity = 2, CategoryId = category.Id });
        await ctx.SaveChangesAsync();
        var repo = new Repository<InventoryDbContext, InventoryItem, Guid>(ctx);
        var sorted = await repo.GetAllAsync(i => i, i => i.CategoryId == category.Id, true, null, null,
            [(i => i.Name, SortingDirection.Descending)], CancellationToken.None);
        Assert.Equal("b", sorted[0].Name);
    }

    [Fact]
    public async Task CountAsync_ReturnsCount()
    {
        await using var ctx = CreateDbContext();
        ctx.InventoryItems.Add(new InventoryItem { Name = "item1", StockQuantity = 1, CategoryId = Guid.NewGuid() });
        await ctx.SaveChangesAsync();
        var repo = new Repository<InventoryDbContext, InventoryItem, Guid>(ctx);
        var count = await repo.CountAsync(null, CancellationToken.None);
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task GetFirstAsync_ReturnsFirstOrNull()
    {
        await using var ctx = CreateDbContext();
        var category = new Category { Name = "cat1" };
        ctx.Categories.Add(category);
        await ctx.SaveChangesAsync();
        ctx.InventoryItems.Add(new InventoryItem { Name = "item1", StockQuantity = 1, CategoryId = category.Id });
        await ctx.SaveChangesAsync();
        var repo = new Repository<InventoryDbContext, InventoryItem, Guid>(ctx);
        var first = await repo.GetFirstAsync(i => i, null, true, CancellationToken.None);
        Assert.NotNull(first);
        var none = await repo.GetFirstAsync(i => i, i => i.Name == "none", true, CancellationToken.None);
        Assert.Null(none);
    }

    [Fact]
    public async Task GetSingleAsync_ReturnsSingleOrNull()
    {
        await using var ctx = CreateDbContext();
        var category = new Category { Name = "cat1" };
        ctx.Categories.Add(category);
        await ctx.SaveChangesAsync();
        ctx.InventoryItems.Add(new InventoryItem { Name = "item1", StockQuantity = 1, CategoryId = category.Id });
        await ctx.SaveChangesAsync();
        var repo = new Repository<InventoryDbContext, InventoryItem, Guid>(ctx);
        var single = await repo.GetSingleAsync(i => i, i => i.Name == "item1", true, CancellationToken.None);
        Assert.NotNull(single);
        var none = await repo.GetSingleAsync(i => i, i => i.Name == "none", true, CancellationToken.None);
        Assert.Null(none);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEntity()
    {
        await using var ctx = CreateDbContext();
        var category = new Category { Name = "cat1" };
        ctx.Categories.Add(category);
        await ctx.SaveChangesAsync();
        var item = new InventoryItem { Name = "old", StockQuantity = 1, CategoryId = category.Id };
        ctx.InventoryItems.Add(item);
        await ctx.SaveChangesAsync();
        ctx.Entry(item).State = EntityState.Detached;
        var repo = new Repository<InventoryDbContext, InventoryItem, Guid>(ctx);
        await repo.UpdateAsync(item.Id, i => i.Name = "new", CancellationToken.None);
        var updated = await ctx.InventoryItems.FindAsync(item.Id);
        Assert.Equal("new", updated.Name);
    }

    [Fact]
    public async Task DeleteAsync_RemovesEntity()
    {
        await using var ctx = CreateDbContext();
        var item = new InventoryItem { Name = "del", StockQuantity = 1, CategoryId = Guid.NewGuid() };
        ctx.InventoryItems.Add(item);
        await ctx.SaveChangesAsync();
        var repo = new Repository<InventoryDbContext, InventoryItem, Guid>(CreateDbContext());
        await repo.DeleteAsync(item.Id, CancellationToken.None);
        Assert.False(ctx.InventoryItems.Any(i => i.Id == item.Id));
    }

    [Fact]
    public async Task AnyAsync_ReturnsCorrectly()
    {
        await using var ctx = CreateDbContext();
        ctx.InventoryItems.Add(new InventoryItem { Name = "item1", StockQuantity = 1, CategoryId = Guid.NewGuid() });
        await ctx.SaveChangesAsync();
        var repo = new Repository<InventoryDbContext, InventoryItem, Guid>(ctx);
        var any = await repo.AnyAsync(i => i.Name == "item1", CancellationToken.None);
        Assert.True(any);
        var none = await repo.AnyAsync(i => i.Name == "none", CancellationToken.None);
        Assert.False(none);
    }
}
