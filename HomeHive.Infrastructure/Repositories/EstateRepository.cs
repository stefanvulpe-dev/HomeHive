using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class EstateRepository: BaseRepository<Estate>
{
    public EstateRepository(HomeHiveContext context) : base(context)
    {
    }
}