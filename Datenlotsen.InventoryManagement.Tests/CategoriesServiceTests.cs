using Moq;
using System.Linq.Expressions;
using Datenlotsen.InventoryManagement.Models;
using Datenlotsen.InventoryManagement.Data.Models;
using Datenlotsen.InventoryManagement.Shared.Data;

namespace Datenlotsen.InventoryManagement.Tests;

public class CategoriesServiceTests
{
    private readonly Mock<IRepository<Category, Guid>> _categoriesRepositoryMock;
    private readonly CategoriesService _service;

    public CategoriesServiceTests()
    {
        _categoriesRepositoryMock = new Mock<IRepository<Category, Guid>>();
        _service = new CategoriesService(_categoriesRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCategories()
    {
        var expected = new List<CategoryModel> { new CategoryModel { Id = Guid.NewGuid(), Name = "cat1" } };
        _categoriesRepositoryMock.Setup(r => r.GetAllAsync<CategoryModel>(It.IsAny<Expression<Func<Category, CategoryModel>>>(), null, true, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var result = await _service.GetAllAsync(CancellationToken.None);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoCategories()
    {
        _categoriesRepositoryMock.Setup(r => r.GetAllAsync<CategoryModel>(It.IsAny<Expression<Func<Category, CategoryModel>>>(), null, true, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CategoryModel>());
        var result = await _service.GetAllAsync(CancellationToken.None);
        Assert.Empty(result);
    }
}
