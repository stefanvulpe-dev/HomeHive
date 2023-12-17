using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Infrastructure.Repositories;

public class ContractRepository(HomeHiveContext context) : BaseRepository<Contract>(context), IContractRepository
{
    public async Task<Result<IReadOnlyList<Contract>>> GetContractsByUserId(Guid userId)
    {
        var result = await Context.Set<Contract>().Where(c => c.UserId == userId).ToListAsync();
        return Result<IReadOnlyList<Contract>>.Success(result);
    }
}