namespace Shared.Repositories;

public class SongRepository(TableServiceClient service) : Repository<Song>(service, "Songs"){

}