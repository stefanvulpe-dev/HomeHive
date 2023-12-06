using HomeHive.Application.Features.Users.Commands.CreateEstate;
using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstate;

public class UpdateEstateCommandResponse : BaseResponse
{
    public UpdateEstateCommandResponse() : base()
    {
    }
    
    public CreateEstateDto? Estate { get; set; } = default!;
}