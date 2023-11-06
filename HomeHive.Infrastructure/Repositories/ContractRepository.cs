using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class ContractRepository : BaseRepository<Contract>
{
    public ContractRepository(HomeHiveContext context) : base(context)
    {
        
    }
}