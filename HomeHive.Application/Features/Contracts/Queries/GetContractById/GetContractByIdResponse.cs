using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Contracts.Queries.GetContractById;

public class GetContractByIdResponse : BaseResponse
{
    public ContractDto? Contract { get; set; }
}