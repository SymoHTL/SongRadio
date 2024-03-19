namespace Shared.Repositories;

public interface IRepository<T> where T : class {
    IAsyncEnumerable<T> ReadAsync(CancellationToken ct);
    Task<List<T>> ReadAsync(int skip, int take, CancellationToken ct);
    Task<T?> ReadAsync(string id, CancellationToken ct);
    Task<IEnumerable<T>> ReadAsync(IEnumerable<string> ids, CancellationToken ct);
    Task<IEnumerable<T>> ReadAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<T> CreateAsync(T entity, CancellationToken ct);
    Task<T> CreateOrUpdateAsync(T entity, CancellationToken ct);
    Task CreateOrUpdateAsync(IEnumerable<T> entities, CancellationToken ct);
    Task<T> UpdateAsync(T entity, CancellationToken ct);
    Task<T> DeleteAsync(string id, CancellationToken ct);
}