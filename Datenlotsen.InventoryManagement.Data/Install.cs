using Datenlotsen.InventoryManagement.Data.Models;
using Datenlotsen.InventoryManagement.Shared.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Datenlotsen.InventoryManagement.Data;

public static class Install
{
    public static IServiceCollection AddInventoryManagementData(this IServiceCollection services, string connectionString)
    {
        return services
            .AddRepository<InventoryDbContext, InventoryItem, Guid>(connectionString)
            .AddRepository<InventoryDbContext, Category, Guid>(connectionString);
    }
}