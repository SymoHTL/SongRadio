namespace Api.Repositories;

public interface IRepository<T> where T : class {
    Task<List<T>> ReadAsync(int skip, int take, CancellationToken ct);
    Task<T> ReadAsync(int id, CancellationToken ct);
    Task<T> CreateAsync(Song song, CancellationToken ct);
    Task<T> UpdateAsync(string id, Song song, CancellationToken ct);
    Task<T> DeleteAsync(string id, CancellationToken ct);
}