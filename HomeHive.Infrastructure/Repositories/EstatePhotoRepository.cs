using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class EstatePhotoRepository : BaseRepository<EstatePhoto>, IEstatePhotoRepository
{
    public EstatePhotoRepository(HomeHiveContext context) : base(context)
    {
    }
}