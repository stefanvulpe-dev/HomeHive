using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Persistence;

public interface IUserRepository : IAsyncRepository<User>
{
    public Task<Result<User>> GetByEmailAsync(string email);
}