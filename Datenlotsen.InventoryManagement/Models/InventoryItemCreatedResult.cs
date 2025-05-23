namespace Datenlotsen.InventoryManagement.Models;

public record InventoryItemCreatedResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal StockQuantity { get; set; }
}