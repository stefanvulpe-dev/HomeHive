using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Infrastructure.Repositories;

public class EstatePhotosRepository(HomeHiveContext context)
    : BaseRepository<EstatePhoto>(context), IEstatePhotosRepository
{
    public async Task<Result<IReadOnlyList<EstatePhoto>>> FindAllByEstateIdAsync(Guid estateId)
    {
        var estatePhotos = await Context.Set<EstatePhoto>()
            .Where(x => x.EstateId == estateId)
            .ToListAsync();
            
        return Result<IReadOnlyList<EstatePhoto>>.Success(estatePhotos);
    }

    public async Task<Result> DeleteAllByEstateIdAsync(Guid estateId)
    {
        var estatePhotosResult = await FindAllByEstateIdAsync(estateId);
        if (estatePhotosResult is not { IsSuccess: true }) return Result.Failure("EstatePhotos not found");

        Context.Set<EstatePhoto>().RemoveRange(estatePhotosResult.Value);
        var result = await Context.SaveChangesAsync();
        return result == 0 ? Result.Failure("EstatePhotos could not be deleted") : Result.Success();
    }
}