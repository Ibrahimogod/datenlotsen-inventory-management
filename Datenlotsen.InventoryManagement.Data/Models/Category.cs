using Datenlotsen.InventoryManagement.Shared.Data;

namespace Datenlotsen.InventoryManagement.Data.Models;

public class Category : Entity<Guid>
{
    public string Name { get; set; }
    public ICollection<InventoryItem> InventoryItems { get; set; }
}