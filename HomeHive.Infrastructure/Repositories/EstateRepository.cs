using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class EstateRepository : BaseRepository<Estate>, IEstateRepository
{
    public EstateRepository(HomeHiveContext context) : base(context)
    {
    }
}