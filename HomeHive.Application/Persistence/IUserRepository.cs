using HomeHive.Domain.Entities;

namespace HomeHive.Application.Persistence;

public interface IUserRepository: IAsyncRepository<User>
{
}