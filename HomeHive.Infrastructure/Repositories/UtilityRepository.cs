using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class UtilityRepository : BaseRepository<Utility>, IUtilityRepository
{
    public UtilityRepository(HomeHiveContext context) : base(context)
    {
    }
}