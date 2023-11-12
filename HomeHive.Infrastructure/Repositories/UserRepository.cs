using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Infrastructure.Repositories;

public class UserRepository: BaseRepository<User>, IUserRepository
{
    public UserRepository(HomeHiveContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        if (Context.Users != null)
        {
            return await Context.Set<User>().FirstOrDefaultAsync(u => u!.Email == email);
        }
        return null;
    }
}