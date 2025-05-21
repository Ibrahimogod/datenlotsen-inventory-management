using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Datenlotsen.InventoryManagement.Shared.Data;

public static class Install
{
    public static IServiceCollection AddRepository<TDbContext,TEntity, TId>(this IServiceCollection services, string connectionString)
        where TEntity : Entity<TId>, new()
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(o =>
        {
            o.UseNpgsql(connectionString);
        });
        services.AddScoped<IRepository<TEntity, TId>, Repository<TDbContext, TEntity, TId>>();
        return services;
    }
}