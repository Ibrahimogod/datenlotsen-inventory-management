namespace Datenlotsen.InventoryManagement.Shared.Data;

public interface IEntity<TId>
{
    TId Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}