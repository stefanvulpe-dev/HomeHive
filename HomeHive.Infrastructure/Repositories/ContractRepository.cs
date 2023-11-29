using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class ContractRepository : BaseRepository<Contract>, IContractRepository
{
    public ContractRepository(HomeHiveContext context) : base(context)
    {
    }
}