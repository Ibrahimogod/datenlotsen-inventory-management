using Datenlotsen.WPF.ViewModels;

namespace Datenlotsen.WPF.Services;

public interface IInventoryApiService
{
    Task<List<InventoryItemModel>> GetInventoryItemsAsync(string? name = null,
        StockStatus? stockStatus = null, Guid? categoryId = null, CancellationToken cancellationToken = default);

    Task<List<CategoryModel>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<InventoryItemModel?> GetInventoryItemAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid?> CreateInventoryItemAsync(string name, decimal stockQuantity, Guid categoryId, CancellationToken cancellationToken = default);
    Task<bool> UpdateInventoryItemAsync(Guid id, string name, decimal stockQuantity, Guid categoryId, CancellationToken cancellationToken = default);
    Task<bool> DeleteInventoryItemAsync(Guid id, CancellationToken cancellationToken = default);
}