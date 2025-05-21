using System.Linq.Expressions;

namespace Datenlotsen.InventoryManagement.Shared.Data;

public interface IRepository<TEntity, in TId> 
    where TEntity : class, IEntity<TId>
{
    ValueTask<List<TResult>> GetAllAsync<TResult>(
         Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asNoTracking = true,
        int? skip = null,
        int? limit = null,
        List<(Expression<Func<TEntity, object>> keySelector, SortingDirection SortingDirection)>? sortings = null,
        CancellationToken cancellationToken = default);

    ValueTask<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    
    ValueTask<TResult?> GetFirstAsync<TResult>(
         Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    ValueTask<TResult?> GetSingleAsync<TResult>(
         Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);
    
    ValueTask AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);
    
    ValueTask UpdateAsync(
        TId id,
        Action<TEntity> updateExpression,
        CancellationToken cancellationToken = default);
    
    ValueTask DeleteAsync(TId id, CancellationToken cancellationToken);
    
    ValueTask<bool> AnyAsync(
         Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
}