using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Contracts.Queries.GetAllContractsByOwnerId;

public class GetAllContractsByOwnerIdQueryResponse: BaseResponse
{
    public IReadOnlyList<ContractDto>? Contracts { get; init; }
}