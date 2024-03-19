namespace Shared.Repositories;

public class SongViewRepository(TableServiceClient service) : Repository<SongView>(service, "SongViews") {
}