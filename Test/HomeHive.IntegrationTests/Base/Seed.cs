using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Entities;
using HomeHive.Infrastructure;

namespace IntegrationTests.Base;

public class Seed
{
    public static void InitializeDbForTests(HomeHiveContext context)
    {
        var userId = Guid.NewGuid();
        var contracts = new List<Contract>
        {
            Contract.Create(userId, new ContractData(Guid.NewGuid(), "Rent", DateTime.Now, DateTime.Now, "Description1")).Value,
            Contract.Create(userId, new ContractData(Guid.NewGuid(), "Sale", DateTime.Now, DateTime.Now, "Description2")).Value,
            Contract.Create(userId, new ContractData(Guid.NewGuid(), "Rent", DateTime.Now, DateTime.Now, "Description3")).Value,
            Contract.Create(userId, new ContractData(Guid.NewGuid(), "Sale", DateTime.Now, DateTime.Now, "Description4")).Value,
        };
        
        context.Contracts?.AddRange(contracts);
        context.SaveChanges();
    }
}