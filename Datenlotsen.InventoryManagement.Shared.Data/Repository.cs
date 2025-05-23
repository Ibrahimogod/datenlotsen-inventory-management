using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Datenlotsen.InventoryManagement.Shared.Data;

public class Repository<TDbContext,TEntity,TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>, new()
    where TDbContext : DbContext
{
    public Repository(TDbContext context)
    {
        Context = context;
        Set = Context.Set<TEntity>();
    }
    
    protected DbContext Context { get; }
    protected DbSet<TEntity> Set { get; }
    
    public virtual async ValueTask<List<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector, 
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asNoTracking = true,
        int? skip = null,
        int? limit = null,
        List<(Expression<Func<TEntity, object>> keySelector, SortingDirection SortingDirection)>? sortings = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        if (sortings != null && sortings.Any())
        {
            var orderedQuery = sortings[0].SortingDirection switch
            {
                SortingDirection.Ascending => query.OrderBy(sortings[0].keySelector),
                SortingDirection.Descending => query.OrderByDescending(sortings[0].keySelector),
                _ => throw new NotSupportedException(),
            };

            for (int i = 0; i < sortings.Count; i++)
            {
                var item = sortings[i];
                orderedQuery = item.SortingDirection switch
                {
                    SortingDirection.Ascending => orderedQuery.ThenBy(item.keySelector),
                    SortingDirection.Descending => orderedQuery.ThenByDescending(item.keySelector),
                    _ => throw new NotSupportedException(),
                };
            }

            query = orderedQuery;
        }

        if (skip != null && limit != null)
            query = query.Skip(skip.Value).Take(limit.Value);

        return await query
            .Select(selector)
            .ToListAsync(cancellationToken);
    }

    public virtual async ValueTask<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (predicate != null)
            query = query.Where(predicate);

        return await query.CountAsync(cancellationToken);
    }

    public virtual async ValueTask<TResult?> GetFirstAsync<TResult>(
         Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        return await query
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async ValueTask<TResult?> GetSingleAsync<TResult>(
         Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        return await query
            .Select(selector)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await Set.AddAsync(entity, cancellationToken);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        await Context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public virtual async ValueTask UpdateAsync(
        TId id,
        Action<TEntity> updateExpression,
        CancellationToken cancellationToken = default)
    {
        var entity = new TEntity
        {
            Id = id
        };
        Context.Attach(entity);
        updateExpression(entity);
        entity.UpdatedAt = DateTime.UtcNow;
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async ValueTask DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = new TEntity
        {
            Id = id
        };
        Context.Attach(entity);
        Set.Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async ValueTask<bool> AnyAsync(
         Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        query = query.Where(predicate);

        return await query.AnyAsync(cancellationToken);
    }
}