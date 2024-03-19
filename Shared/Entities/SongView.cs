namespace Shared.Entities;

public class SongView : ITableEntity {
    [Required]
    public string SongId { get; set; } = null!;
    
    public string PartitionKey { get; set; } = "SongView";
    public string RowKey { get; set; } = Guid.NewGuid().ToString();
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}