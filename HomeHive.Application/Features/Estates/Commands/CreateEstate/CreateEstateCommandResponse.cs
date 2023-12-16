using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public class CreateEstateCommandResponse : BaseResponse
{
    public CreateEstateDto Estate { get; set; } = default!;
}