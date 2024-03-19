namespace Shared.DTO;

public class ReadTopSong {
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public int Duration { get; set; }
    public string Genre { get; set; } = null!;
    public int Views { get; set; }
}