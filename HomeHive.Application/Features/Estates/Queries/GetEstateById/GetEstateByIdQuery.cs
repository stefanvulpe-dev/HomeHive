using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Estates.Queries.GetEstateById;

public record GetEstateByIdQuery(Guid Id) : IQuery<GetEstateByIdResponse>;