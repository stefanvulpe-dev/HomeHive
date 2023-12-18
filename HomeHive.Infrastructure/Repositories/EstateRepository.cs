using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Infrastructure.Repositories;

public class EstateRepository : BaseRepository<Estate>, IEstateRepository
{
    public EstateRepository(HomeHiveContext context) : base(context)
    {
    }
    
    public override async Task<Result<Estate>> FindByIdAsync(Guid id)
    {
        var result = await Context.Set<Estate>()
            .Include(x => x.Utilities)
            .Include(x => x.EstateRooms)
            .FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id);

        return result == null
            ? Result<Estate>.Failure($"Entity with id {id} not found")
            : Result<Estate>.Success(result);
    }
    public override async Task<Result<IReadOnlyList<Estate>>> GetAllAsync()
    {
        var result = await Context.Set<Estate>()
            .Include(x => x.Utilities)
            .Include(x => x.EstateRooms)
            .ToListAsync();

        return Result<IReadOnlyList<Estate>>.Success(result);
    }
}