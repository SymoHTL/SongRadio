namespace Aggregator.Consumer;

public class ViewSongMessageConsumer(IRepository<TopSong> topRepository, IRepository<Song> songRepository,
    IRepository<SongView> viewRepository, ILogger<ViewSongMessageConsumer> logger) : IConsumer<ViewSongMessage> {
    
    private static DateTimeOffset _lastUpdated = DateTimeOffset.UtcNow;

    public async Task Consume(ConsumeContext<ViewSongMessage> context) {
        logger.LogInformation("Received view song message");
        if (DateTimeOffset.UtcNow - _lastUpdated > TimeSpan.FromSeconds(30)) await UpdateViews();
    }

    private async Task UpdateViews() {
        _lastUpdated = DateTimeOffset.UtcNow;
        
        logger.LogInformation("Updating top songs");
        
        var dict = new Dictionary<string, int>();
        
        await foreach(var song in viewRepository.ReadAsync(CancellationToken.None)) {
            if (!dict.TryAdd(song.SongId, 1))
                dict[song.SongId]++;
        }
        
        var songs = await songRepository.ReadAsync(dict.Keys, CancellationToken.None);
        var topSongs = songs.Select(s => new TopSong(s) {
            Views = dict[s.RowKey]
        });
        
        await topRepository.CreateOrUpdateAsync(topSongs, CancellationToken.None);
        
        logger.LogInformation("Updated top songs");
    }
}