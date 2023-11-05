using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class PhotoRepository : BaseRepository<Photo>
{
    public PhotoRepository(HomeHiveContext context) : base(context)
    {
        
    }
}