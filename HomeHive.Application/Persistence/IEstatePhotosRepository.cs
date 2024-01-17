using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Persistence;

public interface IEstatePhotosRepository : IAsyncRepository<EstatePhoto>
{
    Task<Result<IReadOnlyList<EstatePhoto>>> FindAllByEstateIdAsync(Guid estateId);
    Task<Result> DeleteAllByEstateIdAsync(Guid estateId);
}