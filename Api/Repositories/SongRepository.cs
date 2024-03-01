using Azure.Data.Tables;

namespace Api.Repositories;

public class SongRepository(TableServiceClient service) : IRepository<Song> {
    private readonly TableClient _table = service.GetTableClient(nameof(Song));

    public async Task<List<Song>> ReadAsync(int skip, int take, CancellationToken ct) {
        var queryResults = _table.QueryAsync<Song>(cancellationToken: ct);
        var entities = new List<Song>();
        var count = 0;

        await foreach (var entity in queryResults) {
            if (count >= skip && entities.Count < take) entities.Add(entity);
            count++;
            if (entities.Count == take) break;
        }

        return entities;
    }

    public async Task<Song> ReadAsync(int id, CancellationToken ct) {
        var query = _table.QueryAsync<Song>(filter: $"Id eq {id}", cancellationToken: ct);
        await foreach (var entity in query) return entity;
        return null!;
    }

    public async Task<Song> CreateAsync(Song song, CancellationToken ct) {
        await _table.AddEntityAsync(song, cancellationToken: ct);
        return song;
    }

    public async Task<Song> UpdateAsync(string id, Song song, CancellationToken ct) {
        await _table.UpdateEntityAsync(song, song.ETag, TableUpdateMode.Replace, cancellationToken: ct);
        return song;
    }

    public async Task<Song> DeleteAsync(string id, CancellationToken ct) {
        var song = await _table.GetEntityAsync<Song>(id, id, cancellationToken: ct);
        await _table.DeleteEntityAsync(song.Value.PartitionKey, song.Value.RowKey, song.Value.ETag, cancellationToken: ct);
        return song.Value;
    }
}