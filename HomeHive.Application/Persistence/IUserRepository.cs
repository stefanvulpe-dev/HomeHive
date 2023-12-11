using System.Threading.Tasks;
using HomeHive.Domain.Common;
using HomeHive.Domain.Models;

namespace HomeHive.Application.Persistence;

public interface IUserRepository
{
    public Task<Result<User>> FindByIdAsync(Guid id);
    public Task<Result> DeleteByIdAsync(Guid id);
    public Task<Result<IReadOnlyList<User>>> GetAllAsync();
}