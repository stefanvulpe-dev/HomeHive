using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Contracts.Queries.GetAllContracts;

public class GetAllContractsQueryResponse : BaseResponse
{
    public IReadOnlyList<ContractDto>? Contracts { get; set; }
}