using HomeHive.Domain.Common;
using HomeHive.Domain.Models;

namespace HomeHive.Application.Persistence;

public interface IUserRepository
{
    Task<Result<User>> FindByIdAsync(Guid id);
    Task<Result> DeleteByIdAsync(Guid id);
    Task<Result<IReadOnlyList<User>>> GetAllAsync();
    Task<Result<User>> UpdateAsync(User user);
}