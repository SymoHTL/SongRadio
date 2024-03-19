namespace Shared.Entities;

public class TopSong : ITableEntity {
    public TopSong(Song song) {
        RowKey = song.Id;
        Genre = song.Genre;
        Title = song.Title;
        Artist = song.Artist;
        Duration = song.Duration;
    }

    public TopSong() {
    }

    public string Id => RowKey;

    [Required] public string Title { get; set; } = null!;

    [Required] public string Artist { get; set; } = null!;

    [Required] public int Duration { get; set; }

    [Required] public string Genre { get; set; } = null!;

    public int Views { get; set; }


    public string PartitionKey { get; set; } = "TopSong";
    public string RowKey { get; set; } = null!;

    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}