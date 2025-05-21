namespace Datenlotsen.InventoryManagement.Models;

public record InventoryItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal StockQuantity { get; set; }
    public CategoryModel Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}