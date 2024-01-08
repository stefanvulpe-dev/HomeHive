using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
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

        var estates = new List<Estate>
        {
            Estate.Create(userId, new List<Utility>(), new EstateData("House", "ForRent", "Name1", "Location1", 20, "40m2", new List<string>{"AAA"}, new Dictionary<string, int>{{"Key1", 2}}, "Description1", "asodjaosjfoajso")).Value,
            Estate.Create(userId, new List<Utility>(), new EstateData("House", "ForSale", "Name2", "Location2", 20, "40m2", new List<string>{"AAA"}, new Dictionary<string, int>{{"Key1", 2}}, "Description2", "asodjaosjfoajso")).Value,
            Estate.Create(userId, new List<Utility>(), new EstateData("House", "ForRent", "Name3", "Location3", 20, "40m2", new List<string>{"AAA"}, new Dictionary<string, int>{{"Key1", 2}}, "Description3", "asodjaosjfoajso")).Value,
        };
        
        var utilities = new List<Utility>()
        {
            Utility.Create("Water").Value,
            Utility.Create("Electricity").Value,
            Utility.Create("Gas").Value,
            Utility.Create("Internet").Value,
            Utility.Create("Cable").Value,
            Utility.Create("Tv").Value,
            Utility.Create("Phone").Value,
            Utility.Create("Heating").Value,
            Utility.Create("Cooling").Value,
        };

        var rooms = new List<Room>()
        {
            Room.Create("Kitchen").Value,
            Room.Create("LivingRoom").Value,
            Room.Create("Bedroom").Value,
            Room.Create("Bathroom").Value
        };
        
        context.Utilities?.AddRange(utilities);
        context.Rooms?.AddRange(rooms);
        context.Contracts?.AddRange(contracts);
        context.Estates?.AddRange(estates);
        context.SaveChanges();
    }
}