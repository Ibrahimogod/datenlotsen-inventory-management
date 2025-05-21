namespace Datenlotsen.InventoryManagement.API.Models;

public record UpdateInventoryItemModel
{
    public string Name { get; set; }
    public decimal StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
}