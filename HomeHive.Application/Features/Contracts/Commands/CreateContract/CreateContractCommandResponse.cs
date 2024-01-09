using HomeHive.Application.Features.Contracts.Queries;
using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Contracts.Commands.CreateContract;

public class CreateContractCommandResponse : BaseResponse
{
    public ContractDto? Contract { get; set; }
}