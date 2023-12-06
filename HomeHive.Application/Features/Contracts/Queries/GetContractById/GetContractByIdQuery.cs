using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Contracts.Queries.GetContractById;

public record GetContractByIdQuery(Guid Id) : IQuery<GetContractByIdResponse>;
