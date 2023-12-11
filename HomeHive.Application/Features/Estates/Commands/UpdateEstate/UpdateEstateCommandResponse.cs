using HomeHive.Application.Features.Estates.Commands.CreateEstate;
using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstate;

public class UpdateEstateCommandResponse : BaseResponse
{
    public CreateEstateDto? Estate { get; set; } = default!;
}