using System.Linq.Expressions;
using Moq;
using Datenlotsen.InventoryManagement.Data.Models;
using Datenlotsen.InventoryManagement.Models;
using Datenlotsen.InventoryManagement.Shared.Data;

namespace Datenlotsen.InventoryManagement.Tests;

public class InventoryServiceTests
{
    private readonly Mock<IRepository<InventoryItem, Guid>> _repoMock;
    private readonly InventoryService _service;

    public InventoryServiceTests()
    {
        _repoMock = new Mock<IRepository<InventoryItem, Guid>>();
        _service = new InventoryService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsItem_WhenFound()
    {
        var id = Guid.NewGuid();
        var expected = new InventoryItemModel { Id = id, Name = "item", StockQuantity = 1, Category = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" } };
        _repoMock.Setup(r => r.GetSingleAsync(It.IsAny<Expression<Func<InventoryItem, InventoryItemModel>>>(),
            It.IsAny<Expression<Func<InventoryItem, bool>>>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var result = await _service.GetByIdAsync(id, CancellationToken.None);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetSingleAsync(It.IsAny<Expression<Func<InventoryItem, InventoryItemModel>>>(), It.IsAny<Expression<Func<InventoryItem, bool>>>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((InventoryItemModel?)null);
        var result = await _service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedResult()
    {
        var id = Guid.NewGuid();
        var name = "item";
        var stock = 5m;
        var catId = Guid.NewGuid();
        var entity = new InventoryItem { Id = id, Name = name, StockQuantity = stock, CategoryId = catId };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<InventoryItem>(), It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        var result = await _service.CreateAsync(name, stock, catId, CancellationToken.None);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(stock, result.StockQuantity);
    }

    [Fact]
    public async Task UpdateAsync_CallsRepository()
    {
        var id = Guid.NewGuid();
        var called = false;
        _repoMock.Setup(r => r.UpdateAsync(id, It.IsAny<Action<InventoryItem>>(), It.IsAny<CancellationToken>()))
            .Callback<Guid, Action<InventoryItem>, CancellationToken>((_, act, __) => { called = true; });
        await _service.UpdateAsync(id, "n", 1, Guid.NewGuid(), CancellationToken.None);
        Assert.True(called);
    }

    [Fact]
    public async Task SearchAsync_CallsRepository()
    {
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<InventoryItem, InventoryItemModel>>>(), It.IsAny<Expression<Func<InventoryItem, bool>>>(), true, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<InventoryItemModel>());
        var result = await _service.SearchAsync(CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepository()
    {
        var id = Guid.NewGuid();
        var called = false;
        _repoMock.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).Callback(() => called = true).Returns(ValueTask.CompletedTask);
        await _service.DeleteAsync(id, CancellationToken.None);
        Assert.True(called);
    }
}
