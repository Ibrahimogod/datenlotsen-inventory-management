using Datenlotsen.InventoryManagement.Enums;
using Datenlotsen.InventoryManagement.Models;

namespace Datenlotsen.InventoryManagement
{
    public interface IInventoryService
    {
        ValueTask<InventoryItemModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        ValueTask<InventoryItemCreatedResult> CreateAsync(string name, decimal stockQuantity, Guid categoryId, CancellationToken cancellationToken);
        ValueTask UpdateAsync(Guid id, string name, decimal stockQuantity, Guid categoryId, CancellationToken cancellationToken);
        ValueTask<List<InventoryItemModel>> SearchAsync(CancellationToken cancellationToken, string? name = null, StockStatus? stockStatus = null);
        ValueTask DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
