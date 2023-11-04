using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class UserRepository: BaseRepository<User>
{
    public UserRepository(HomeHiveContext context) : base(context)
    {
    }
}