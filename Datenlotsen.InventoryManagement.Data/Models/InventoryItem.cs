using Datenlotsen.InventoryManagement.Shared.Data;

namespace Datenlotsen.InventoryManagement.Data.Models
{
    public class InventoryItem : Entity<Guid>
    {
        public string Name { get; set; }
        public decimal StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}