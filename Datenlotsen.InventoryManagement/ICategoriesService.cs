using Datenlotsen.InventoryManagement.Models;

namespace Datenlotsen.InventoryManagement;

public interface ICategoriesService
{
    ValueTask<List<CategoryModel>> GetAllAsync(CancellationToken cancellationToken);
}