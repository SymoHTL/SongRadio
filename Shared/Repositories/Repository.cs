namespace Shared.Repositories;

public class Repository<T>(TableServiceClient service, string name) : IRepository<T> where T : class, ITableEntity{
    private readonly TableClient _table = service.GetTableClient(name);


    public IAsyncEnumerable<T> ReadAsync(CancellationToken ct) {
        return _table.QueryAsync<T>(cancellationToken: ct);
    }

    public async Task<List<T>> ReadAsync(int skip, int take, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        var queryResults = _table.QueryAsync<T>(cancellationToken: ct);
        var entities = new List<T>();
        var count = 0;

        await foreach (var entity in queryResults) {
            if (count >= skip && entities.Count < take) entities.Add(entity);
            count++;
            if (entities.Count == take) break;
        }

        return entities;
    }

    public async Task<T?> ReadAsync(string id, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        var song = await _table.GetEntityAsync<T>(typeof(T).Name, id, cancellationToken: ct);
        return song;
    }

    public async Task<IEnumerable<T>> ReadAsync(IEnumerable<string> ids, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        var entities = new List<T>();
        foreach (var id in ids) {
            var song = await _table.GetEntityAsync<T>(typeof(T).Name, id, cancellationToken: ct);
            if (song != null) entities.Add(song);
        }
        return entities;
    }

    public async Task<IEnumerable<T>> ReadAsync(Expression<Func<T, bool>> predicate, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        var queryResults = _table.QueryAsync(predicate, cancellationToken: ct);
        var entities = new List<T>();

        await foreach (var entity in queryResults) entities.Add(entity);

        return entities;
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        var queryResults = _table.QueryAsync(predicate,cancellationToken: ct);
        await foreach (var entity in queryResults) return entity;
        return null!;
    }

    public async Task<T> CreateAsync(T entity, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        await _table.AddEntityAsync(entity, cancellationToken: ct);
        return entity;
    }

    public async Task<T> CreateOrUpdateAsync(T entity, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        await _table.UpsertEntityAsync(entity, cancellationToken: ct);
        return entity;
    }

    public async Task CreateOrUpdateAsync(IEnumerable<T> entities, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        foreach (var entity in entities) await _table.UpsertEntityAsync(entity, cancellationToken: ct);
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        await _table.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace, cancellationToken: ct);
        return entity;
    }

    public async Task<T> DeleteAsync(string id, CancellationToken ct) {
        await _table.CreateIfNotExistsAsync(ct);
        var song = await _table.GetEntityAsync<T>(typeof(T).Name, id, cancellationToken: ct);
        await _table.DeleteEntityAsync(song.Value.PartitionKey, song.Value.RowKey, song.Value.ETag, cancellationToken: ct);
        return song.Value;
    }
}