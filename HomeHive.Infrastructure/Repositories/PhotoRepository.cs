using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
{
    public PhotoRepository(HomeHiveContext context) : base(context)
    {
    }
}