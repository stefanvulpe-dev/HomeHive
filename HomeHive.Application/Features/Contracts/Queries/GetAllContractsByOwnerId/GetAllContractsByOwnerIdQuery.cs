using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Contracts.Queries.GetAllContractsByOwnerId;

public record GetAllContractsByOwnerIdQuery(Guid OwnerId) : 
    IQuery<GetAllContractsByOwnerIdQueryResponse>;
    