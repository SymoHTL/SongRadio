namespace Shared.Entities;

public class Song : ITableEntity {
    public string Id => RowKey;
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Artist { get; set; } = null!;
    [Required]
    public int Duration { get; set; }
    [Required]
    public string Genre { get; set; } = null!;

    public string PartitionKey { get; set; } = "Song";
    public string RowKey { get; set; } = Guid.NewGuid().ToString();
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}