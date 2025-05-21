using System.Text.Json.Serialization;
using Datenlotsen.InventoryManagement.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Datenlotsen.InventoryManagement.API.Models;

public record SearchInventoryItemModel
{
    [FromQuery]
    public string? Name { get; set; }
    [FromQuery]
    public StockStatus? StockStatus { get; set; }
}