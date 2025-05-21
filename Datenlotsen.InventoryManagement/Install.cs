using Datenlotsen.InventoryManagement.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Datenlotsen.InventoryManagement;

public static class Install
{
    public static IServiceCollection AddInventoryManagementServices(this IServiceCollection services, string connectionString)
    {
        return 
            services
                .AddInventoryManagementData(connectionString)
                .AddScoped<IInventoryService, InventoryService>()
                .AddScoped<ICategoriesService, CategoriesService>();
    }
}