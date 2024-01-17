using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Estates.Queries.GetEstatePhotos;

public record GetEstatePhotosQuery(Guid EstateId) : IQuery<GetEstatePhotosQueryResponse>;