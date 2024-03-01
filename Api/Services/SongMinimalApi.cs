using Microsoft.AspNetCore.Mvc;

namespace Api.Services;

public class SongMinimalApi : IMinimalApiEndpoint {
    public string Route { get; set; } = "/songs";

    public void RegisterRoutes(WebApplication app) {
        app.MapGet(Route,
            async ([FromServices] IRepository<Song> repository, [FromQuery] int skip, [FromQuery] int take,
                    CancellationToken ct) =>
                await repository.ReadAsync(skip, take, ct));

        app.MapGet($"{Route}/{{id:int}}",
            async ([FromServices] IRepository<Song> repository, int id, CancellationToken ct) =>
            await repository.ReadAsync(id, ct));

        app.MapPost(Route, async ([FromServices] IRepository<Song> repository, Song song, CancellationToken ct) =>
        await repository.CreateAsync(song, ct));

        app.MapPut($"{Route}/{{id}}",
            async ([FromServices] IRepository<Song> repository, string id, Song song, CancellationToken ct) =>
            await repository.UpdateAsync(id, song, ct));

        app.MapDelete($"{Route}/{{id}}",
            async ([FromServices] IRepository<Song> repository, string id, CancellationToken ct) =>
            await repository.DeleteAsync(id, ct));
    }
}