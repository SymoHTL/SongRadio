namespace Shared.Repositories;

public class TopSongRepository(TableServiceClient service) : Repository<TopSong>(service, "TopSongs"){

}