using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;

public record GetAllContractsByUserIdQuery(Guid UserId) : IQuery<GetAllContractsByUserIdResponse>;
