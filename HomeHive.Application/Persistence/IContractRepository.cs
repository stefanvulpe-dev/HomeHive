using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Persistence;

public interface IContractRepository : IAsyncRepository<Contract>
{
    Task<Result<IReadOnlyList<Contract>>> GetContractsByUserId(Guid userId);
}