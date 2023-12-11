using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Contracts.Commands.CreateContract;

public class CreateContractCommandResponse : BaseResponse
{
    public CreateContractDto? Contract { get; set; }
}