using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Moq;
using Xunit;
using Datenlotsen.WPF.ViewModels;
using Datenlotsen.WPF.Services;

namespace Datenlotsen.WPF.Tests;

public class MainViewModelTests
{
    private readonly Mock<IInventoryApiService> _apiServiceMock;
    private readonly MainViewModelForTest _vm;

    public MainViewModelTests()
    {
        _apiServiceMock = new Mock<IInventoryApiService>();
        _vm = new MainViewModelForTest(_apiServiceMock.Object);
    }

    [Fact]
    public async Task LoadCategoriesAsync_PopulatesCategories()
    {
        _apiServiceMock.Setup(s => s.GetCategoriesAsync(default)).ReturnsAsync(new[] { new CategoryModel { Id = Guid.NewGuid(), Name = "cat" } }.ToList());
        await _vm.InvokeLoadCategoriesAsync();
        Assert.Contains(_vm.Categories, c => c.Name == "cat");
    }

    [Fact]
    public async Task LoadInventoryAsync_PopulatesInventoryItems()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _vm.Categories.Add(cat);
        _vm.SelectedCategory = cat;
        _apiServiceMock.Setup(s => s.GetInventoryItemsAsync(null, null, cat.Id, default)).ReturnsAsync(new[] { new InventoryItemModel { Id = Guid.NewGuid(), Name = "item", Category = cat } }.ToList());
        await _vm.InvokeLoadInventoryAsync();
        Assert.Contains(_vm.InventoryItems, i => i.Name == "item");
    }

    [Fact]
    public void StartAdd_SetsIsEditingAndSelectedItem()
    {
        _vm.Categories.Add(new CategoryModel { Id = Guid.NewGuid(), Name = "cat" });
        _vm.InvokeStartAdd();
        Assert.True(_vm.IsEditing);
        Assert.NotNull(_vm.SelectedItem);
    }

    [Fact]
    public void StartEdit_ClonesSelectedItem()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _vm.Categories.Add(cat);
        _vm.SelectedItem = new InventoryItemModel { Id = Guid.NewGuid(), Name = "item", Category = cat };
        _vm.InvokeStartEdit();
        Assert.True(_vm.IsEditing);
        Assert.NotNull(_vm.SelectedItem);
        Assert.Equal("item", _vm.SelectedItem.Name);
    }

    [Fact]
    public async Task SaveAsync_CreatesOrUpdatesItem()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _vm.Categories.Add(cat);
        _vm.SelectedItem = new InventoryItemModel { Id = Guid.Empty, Name = "item", StockQuantity = 1, Category = cat };
        _apiServiceMock.Setup(s => s.CreateInventoryItemAsync("item", 1, cat.Id, default)).ReturnsAsync(Guid.NewGuid());
        await _vm.InvokeSaveAsync();
        Assert.False(_vm.IsEditing);
        _vm.SelectedItem = new InventoryItemModel { Id = Guid.NewGuid(), Name = "item2", StockQuantity = 2, Category = cat };
        _apiServiceMock.Setup(s => s.UpdateInventoryItemAsync(It.IsAny<Guid>(), "item2", 2, cat.Id, default)).ReturnsAsync(true);
        await _vm.InvokeSaveAsync();
        Assert.False(_vm.IsEditing);
    }

    [Fact]
    public async Task SaveAsync_SetsErrorMessage_OnException()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _vm.Categories.Add(cat);
        _vm.SelectedItem = new InventoryItemModel { Id = Guid.Empty, Name = "item", StockQuantity = 1, Category = cat };
        _apiServiceMock.Setup(s => s.CreateInventoryItemAsync("item", 1, cat.Id, default)).ThrowsAsync(new Exception("fail"));
        await _vm.InvokeSaveAsync();
        Assert.Equal("fail", _vm.ErrorMessage);
    }

    [Fact]
    public async Task DeleteAsync_DeletesItem()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _vm.Categories.Add(cat);
        _vm.SelectedItem = new InventoryItemModel { Id = Guid.NewGuid(), Name = "item", Category = cat };
        _apiServiceMock.Setup(s => s.DeleteInventoryItemAsync(_vm.SelectedItem.Id, CancellationToken.None)).ReturnsAsync(true);
        // Mock inventory load to avoid null reference
        _apiServiceMock.Setup(s => s.GetInventoryItemsAsync(It.IsAny<string>(), It.IsAny<StockStatus?>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<InventoryItemModel>());
        await _vm.InvokeDeleteAsync();
        Assert.Null(_vm.ErrorMessage);
    }

    [Fact]
    public async Task DeleteAsync_SetsErrorMessage_OnException()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _vm.Categories.Add(cat);
        _vm.SelectedItem = new InventoryItemModel { Id = Guid.NewGuid(), Name = "item", Category = cat };
        _apiServiceMock.Setup(s => s.DeleteInventoryItemAsync(_vm.SelectedItem.Id, default)).ThrowsAsync(new Exception("fail"));
        await _vm.InvokeDeleteAsync();
        Assert.Equal("fail", _vm.ErrorMessage);
    }

    [Fact]
    public void CancelEdit_ResetsIsEditingAndErrorMessage()
    {
        _vm.IsEditing = true;
        _vm.ErrorMessage = "err";
        _vm.InvokeCancelEdit();
        Assert.False(_vm.IsEditing);
        Assert.Null(_vm.ErrorMessage);
    }

    [Fact]
    public void CanSave_ReturnsExpected()
    {
        _vm.SelectedItem = new InventoryItemModel { Name = "", StockQuantity = 1 };
        Assert.False(_vm.InvokeCanSave());
        _vm.SelectedItem = new InventoryItemModel { Name = "ok", StockQuantity = 1 };
        Assert.True(_vm.InvokeCanSave());
    }

    // Helper subclass to expose protected/private methods for testing
    private class MainViewModelForTest : MainViewModel
    {
        public MainViewModelForTest(IInventoryApiService apiService)
        {
            typeof(MainViewModel)
                .GetField("_apiService", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(this, apiService);
        }
        public Task InvokeLoadCategoriesAsync() => (Task)typeof(MainViewModel).GetMethod("LoadCategoriesAsync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(this, null);
        public Task InvokeLoadInventoryAsync() => (Task)typeof(MainViewModel).GetMethod("LoadInventoryAsync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(this, null);
        public void InvokeStartAdd() => typeof(MainViewModel).GetMethod("StartAdd", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(this, null);
        public void InvokeStartEdit() => typeof(MainViewModel).GetMethod("StartEdit", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(this, null);
        public Task InvokeSaveAsync() => (Task)typeof(MainViewModel).GetMethod("SaveAsync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(this, null);
        public Task InvokeDeleteAsync() => (Task)typeof(MainViewModel).GetMethod("DeleteAsync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(this, null);
        public void InvokeCancelEdit() => typeof(MainViewModel).GetMethod("CancelEdit", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(this, null);
        public bool InvokeCanSave() => (bool)typeof(MainViewModel).GetMethod("CanSave", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(this, null);
    }
}
