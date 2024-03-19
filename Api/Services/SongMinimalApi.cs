namespace Api.Services;

public class SongMinimalApi : IMinimalApiEndpoint {
    public string Route => "/songs";

    public void RegisterRoutes(WebApplication app) {
        app.MapGet(Route,
            async ([FromServices] IRepository<Song> repository,
                    [FromQuery] int skip, [FromQuery] int take, CancellationToken ct) =>
                (await repository.ReadAsync(skip, take, ct))
                .Select(s => s.Adapt<ReadSongDto>()));

        app.MapGet($"{Route}/{{id}}",
            async ([FromServices] IRepository<Song> repository, string id, CancellationToken ct) =>
            (await repository.ReadAsync(id, ct))
            .Adapt<ReadSongDto>());

        app.MapGet($"{Route}/genre/{{genre}}",
            async ([FromServices] IRepository<TopSong> repository, string genre, CancellationToken ct) =>
            (await repository.ReadAsync(s => s.Genre == genre, ct))
            .Select(s => s.Adapt<ReadTopSong>()));

        app.MapPost(Route,
            async ([FromServices] IRepository<Song> repository, CreateSongDto song, CancellationToken ct) =>
            (await repository.CreateAsync(song.Adapt<Song>(), ct))
            .Adapt<ReadSongDto>());

        app.MapPost($"{Route}/{{id}}", async ([FromServices] IRepository<SongView> viewRepository,
            [FromServices] IRepository<Song> songRepository,
            [FromServices] IPublishEndpoint publisher, string id, CancellationToken ct) => {
            await viewRepository.CreateAsync(new SongView { SongId = id }, ct);
            await publisher.Publish(new ViewSongMessage(), ct);
            return Results.Accepted();
        });

        app.MapPut($"{Route}/{{id}}",
            async ([FromServices] IRepository<Song> repository, string id, UpdateSongDto song,
                CancellationToken ct) => {
                var entity = song.Adapt<Song>();
                entity.RowKey = id;
                (await repository.UpdateAsync(entity, ct))
                    .Adapt<ReadSongDto>();
            });

        app.MapDelete($"{Route}/{{id}}",
            async ([FromServices] IRepository<Song> repository, string id, CancellationToken ct) =>
            (await repository.DeleteAsync(id, ct))
            .Adapt<ReadSongDto>());
    }
}