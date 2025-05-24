using Moq;
using Datenlotsen.WPF.ViewModels;
using Datenlotsen.WPF.Services;

namespace Datenlotsen.WPF.Tests;

public class MainViewModelTests
{
    private readonly Mock<IInventoryApiService> _apiServiceMock;
    private readonly MainViewModelForTest _viewModel;

    public MainViewModelTests()
    {
        _apiServiceMock = new Mock<IInventoryApiService>();
        _viewModel = new MainViewModelForTest(_apiServiceMock.Object);
    }

    [Fact]
    public async Task LoadCategoriesAsync_PopulatesCategories()
    {
        _apiServiceMock.Setup(s => s.GetCategoriesAsync(default)).ReturnsAsync(new[] { new CategoryModel { Id = Guid.NewGuid(), Name = "cat" } }.ToList());
        await _viewModel.InvokeLoadCategoriesAsync();
        Assert.Contains(_viewModel.Categories, c => c.Name == "cat");
    }

    [Fact]
    public async Task LoadInventoryAsync_PopulatesInventoryItems()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _viewModel.Categories.Add(cat);
        _viewModel.SelectedCategory = cat;
        _apiServiceMock.Setup(s => s.GetInventoryItemsAsync(null, null, cat.Id, default)).ReturnsAsync(new[] { new InventoryItemModel { Id = Guid.NewGuid(), Name = "item", Category = cat } }.ToList());
        await _viewModel.InvokeLoadInventoryAsync();
        Assert.Contains(_viewModel.InventoryItems, i => i.Name == "item");
    }

    [Fact]
    public void StartAdd_SetsIsEditingAndSelectedItem()
    {
        _viewModel.Categories.Add(new CategoryModel { Id = Guid.NewGuid(), Name = "cat" });
        _viewModel.InvokeStartAdd();
        Assert.True(_viewModel.IsEditing);
        Assert.NotNull(_viewModel.SelectedItem);
    }

    [Fact]
    public void StartEdit_ClonesSelectedItem()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _viewModel.Categories.Add(cat);
        _viewModel.SelectedItem = new InventoryItemModel { Id = Guid.NewGuid(), Name = "item", Category = cat };
        _viewModel.InvokeStartEdit();
        Assert.True(_viewModel.IsEditing);
        Assert.NotNull(_viewModel.SelectedItem);
        Assert.Equal("item", _viewModel.SelectedItem.Name);
    }

    [Fact]
    public async Task SaveAsync_CreatesOrUpdatesItem()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _viewModel.Categories.Add(cat);
        _viewModel.SelectedItem = new InventoryItemModel { Id = Guid.Empty, Name = "item", StockQuantity = 1, Category = cat };
        _apiServiceMock.Setup(s => s.CreateInventoryItemAsync("item", 1, cat.Id, default)).ReturnsAsync(Guid.NewGuid());
        await _viewModel.InvokeSaveAsync();
        Assert.False(_viewModel.IsEditing);
        _viewModel.SelectedItem = new InventoryItemModel { Id = Guid.NewGuid(), Name = "item2", StockQuantity = 2, Category = cat };
        _apiServiceMock.Setup(s => s.UpdateInventoryItemAsync(It.IsAny<Guid>(), "item2", 2, cat.Id, default)).ReturnsAsync(true);
        await _viewModel.InvokeSaveAsync();
        Assert.False(_viewModel.IsEditing);
    }

    [Fact]
    public async Task SaveAsync_SetsErrorMessage_OnException()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _viewModel.Categories.Add(cat);
        _viewModel.SelectedItem = new InventoryItemModel { Id = Guid.Empty, Name = "item", StockQuantity = 1, Category = cat };
        _apiServiceMock.Setup(s => s.CreateInventoryItemAsync("item", 1, cat.Id, default)).ThrowsAsync(new Exception("fail"));
        await _viewModel.InvokeSaveAsync();
        Assert.Equal("fail", _viewModel.ErrorMessage);
    }

    [Fact]
    public async Task DeleteAsync_DeletesItem()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _viewModel.Categories.Add(cat);
        _viewModel.SelectedItem = new InventoryItemModel { Id = Guid.NewGuid(), Name = "item", Category = cat };
        _apiServiceMock.Setup(s => s.DeleteInventoryItemAsync(_viewModel.SelectedItem.Id, CancellationToken.None)).ReturnsAsync(true);
        // Mock inventory load to avoid null reference
        _apiServiceMock.Setup(s => s.GetInventoryItemsAsync(It.IsAny<string>(), It.IsAny<StockStatus?>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<InventoryItemModel>());
        await _viewModel.InvokeDeleteAsync();
        Assert.Null(_viewModel.ErrorMessage);
    }

    [Fact]
    public async Task DeleteAsync_SetsErrorMessage_OnException()
    {
        var cat = new CategoryModel { Id = Guid.NewGuid(), Name = "cat" };
        _viewModel.Categories.Add(cat);
        _viewModel.SelectedItem = new InventoryItemModel { Id = Guid.NewGuid(), Name = "item", Category = cat };
        _apiServiceMock.Setup(s => s.DeleteInventoryItemAsync(_viewModel.SelectedItem.Id, default)).ThrowsAsync(new Exception("fail"));
        await _viewModel.InvokeDeleteAsync();
        Assert.Equal("fail", _viewModel.ErrorMessage);
    }

    [Fact]
    public void CancelEdit_ResetsIsEditingAndErrorMessage()
    {
        _viewModel.IsEditing = true;
        _viewModel.ErrorMessage = "err";
        _viewModel.InvokeCancelEdit();
        Assert.False(_viewModel.IsEditing);
        Assert.Null(_viewModel.ErrorMessage);
    }

    [Fact]
    public void CanSave_ReturnsExpected()
    {
        _viewModel.SelectedItem = new InventoryItemModel { Name = "", StockQuantity = 1 };
        Assert.False(_viewModel.InvokeCanSave());
        _viewModel.SelectedItem = new InventoryItemModel { Name = "ok", StockQuantity = 1 };
        Assert.True(_viewModel.InvokeCanSave());
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
