using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Features.Contracts.Queries;
using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Contracts.Commands.UpdateContract;

public class UpdateContractCommandResponse : BaseResponse
{
    public ContractDto? Contract { get; set; }
}