namespace Datenlotsen.InventoryManagement.API.Models;

public record CreateInventoryItemModel
{
    public string Name { get; set; }
    public decimal StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
}