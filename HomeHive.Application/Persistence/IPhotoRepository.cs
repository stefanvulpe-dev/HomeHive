using HomeHive.Domain.Entities;

namespace HomeHive.Application.Persistence;

public interface IPhotoRepository : IAsyncRepository<Photo>
{
}