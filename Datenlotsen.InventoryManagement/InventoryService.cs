using Datenlotsen.InventoryManagement.Data.Models;
using Datenlotsen.InventoryManagement.Enums;
using Datenlotsen.InventoryManagement.Models;
using Datenlotsen.InventoryManagement.Shared.Data;

namespace Datenlotsen.InventoryManagement
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepository<InventoryItem, Guid> _inventoryItemRepository;
        public InventoryService(IRepository<InventoryItem,Guid> inventoryItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
        }
        
        public ValueTask<InventoryItemModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _inventoryItemRepository.GetSingleAsync(
                selector: i => new InventoryItemModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    StockQuantity = i.StockQuantity,
                    Category = new CategoryModel
                    {
                        Id = i.Category.Id,
                        Name = i.Category.Name
                    },
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt
                },
                predicate: i => i.Id == id, cancellationToken: cancellationToken);
        }

        public async ValueTask<Guid> CreateAsync(string name, decimal stockQuantity, Guid categoryId, CancellationToken cancellationToken)
        {
            var inventoryItem = new InventoryItem
            {
                Name = name,
                StockQuantity = stockQuantity,
                CategoryId = categoryId
            };

            await _inventoryItemRepository.AddAsync(inventoryItem, cancellationToken);
            return inventoryItem.Id;
        }

        public ValueTask UpdateAsync(Guid id, string name, decimal stockQuantity, Guid categoryId, CancellationToken cancellationToken)
        {
            return _inventoryItemRepository.UpdateAsync(id, item =>
            {
                item.Name = name;
                item.StockQuantity = stockQuantity;
                item.CategoryId = categoryId;
            }, cancellationToken);
        }

        public ValueTask<List<InventoryItemModel>> SearchAsync(CancellationToken cancellationToken, string? name = null, StockStatus? stockStatus = null)
        {
            return _inventoryItemRepository.GetAllAsync(
                selector: i => new InventoryItemModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    StockQuantity = i.StockQuantity,
                    Category = new CategoryModel
                    {
                        Id = i.Category.Id,
                        Name = i.Category.Name
                    },
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt
                },
                predicate: i => (string.IsNullOrEmpty(name) || i.Name.Contains(name)) &&
                               (!stockStatus.HasValue || (stockStatus == StockStatus.LowStock && i.StockQuantity < 5) ||
                                (stockStatus == StockStatus.InStock && i.StockQuantity >= 5)),
                cancellationToken: cancellationToken);
        }

        public ValueTask DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            return _inventoryItemRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
