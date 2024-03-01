using Azure;
using Azure.Data.Tables;

namespace Api.Entities;

public class Song : ITableEntity {

    public int Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public TimeSpan Duration { get; set; }
    public string Genre { get; set; }
    
    
    public string PartitionKey { get; set; } = "Songs";
    public string RowKey { get; set; }  
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}