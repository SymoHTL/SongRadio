namespace Shared.DTO;

public record ReadSongDto {
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public int Duration { get; set; }
    public string Genre { get; set; } = null!;
}

public record CreateSongDto {
    public string Title { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public int Duration { get; set; }
    public string Genre { get; set; } = null!;
}

public class UpdateSongDto {
    public string Title { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public int Duration { get; set; }
    public string Genre { get; set; } = null!;
}