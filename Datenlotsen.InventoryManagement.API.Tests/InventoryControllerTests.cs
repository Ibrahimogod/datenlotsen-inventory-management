using Moq;
using Microsoft.AspNetCore.Mvc;
using Datenlotsen.InventoryManagement.API.Controllers;
using Datenlotsen.InventoryManagement.API.Models;
using Datenlotsen.InventoryManagement.Enums;
using Datenlotsen.InventoryManagement.Models;


namespace Datenlotsen.InventoryManagement.API.Tests;

public class InventoryControllerTests
{
    private readonly Mock<IInventoryService> _serviceMock;
    private readonly InventoryController _controller;

    public InventoryControllerTests()
    {
        _serviceMock = new Mock<IInventoryService>();
        _controller = new InventoryController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithResults()
    {
        _serviceMock.Setup(s => s.SearchAsync(It.IsAny<CancellationToken>(), It.IsAny<string>(), It.IsAny<StockStatus?>(), It.IsAny<Guid?>()))
            .ReturnsAsync(new List<InventoryItemModel> { new InventoryItemModel { Id = Guid.NewGuid(), Name = "item" } });
        var result = await _controller.GetAll(new SearchInventoryItemModel(), CancellationToken.None);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<List<InventoryItemModel>>(ok.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(new InventoryItemModel { Id = id });
        var result = await _controller.GetById(id, CancellationToken.None);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<InventoryItemModel>(ok.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNull()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((InventoryItemModel?)null);
        var result = await _controller.GetById(Guid.NewGuid(), CancellationToken.None);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Add_ReturnsOk_WhenSuccess()
    {
        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new InventoryItemCreatedResult { Id = Guid.NewGuid(), Name = "item", StockQuantity = 1 });
        var result = await _controller.Add(new CreateInventoryItemModel { Name = "item", StockQuantity = 1, CategoryId = Guid.NewGuid() }, CancellationToken.None);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Add_ReturnsBadRequest_OnDbUpdateException()
    {
        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Microsoft.EntityFrameworkCore.DbUpdateException());
        var result = await _controller.Add(new CreateInventoryItemModel { Name = "item", StockQuantity = 1, CategoryId = Guid.NewGuid() }, CancellationToken.None);
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(bad.Value);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenFound()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(new InventoryItemModel { Id = id });
        var result = await _controller.Update(id, new UpdateInventoryItemModel { Name = "n", StockQuantity = 1, CategoryId = Guid.NewGuid() }, CancellationToken.None);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((InventoryItemModel?)null);
        var result = await _controller.Update(Guid.NewGuid(), new UpdateInventoryItemModel { Name = "n", StockQuantity = 1, CategoryId = Guid.NewGuid() }, CancellationToken.None);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenFound()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(new InventoryItemModel { Id = id });
        var result = await _controller.Delete(id, CancellationToken.None);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((InventoryItemModel?)null);
        var result = await _controller.Delete(Guid.NewGuid(), CancellationToken.None);
        Assert.IsType<NotFoundResult>(result);
    }
}
