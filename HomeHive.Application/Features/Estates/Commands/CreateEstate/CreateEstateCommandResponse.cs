using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Users.Commands.CreateEstate;

public class CreateEstateCommandResponse : BaseResponse
{
    public CreateEstateCommandResponse() : base()
    {
    }
    
    public CreateEstateDto Estate { get; set; } = default!;
}