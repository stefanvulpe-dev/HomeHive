using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class UserRepository: BaseRepository<User>, IUserRepository
{
    public UserRepository(HomeHiveContext context) : base(context)
    {
    }
}