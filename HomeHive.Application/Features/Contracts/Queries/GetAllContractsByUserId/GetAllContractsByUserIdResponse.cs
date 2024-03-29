using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;

public class GetAllContractsByUserIdResponse : BaseResponse
{
    public IReadOnlyList<ContractDto>? Contracts { get; set; }
}