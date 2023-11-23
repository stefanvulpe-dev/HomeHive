using HomeHive.Domain.Entities;

namespace HomeHive.Application.Persistence;

public interface IUserRepository: IAsyncRepository<User>
{
    public Task<User?> GetByEmailAsync(string email);
    
    public Task DeleteByEmailAsync(string email);
}