using Datenlotsen.InventoryManagement.Data.Models;
using Datenlotsen.InventoryManagement.Models;
using Datenlotsen.InventoryManagement.Shared.Data;

namespace Datenlotsen.InventoryManagement;

public class CategoriesService : ICategoriesService
{
    private readonly IRepository<Category,Guid> _categoriesRepository;
    public CategoriesService(IRepository<Category,Guid>  categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }
    
    public ValueTask<List<CategoryModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _categoriesRepository.GetAllAsync(
            selector: c => new CategoryModel
            {
                Id = c.Id,
                Name = c.Name
            },
            cancellationToken: cancellationToken);
    }
}