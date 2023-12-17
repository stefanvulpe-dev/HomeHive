using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class EstatePhotoRepository(HomeHiveContext context)
    : BaseRepository<EstatePhoto>(context), IEstatePhotoRepository;