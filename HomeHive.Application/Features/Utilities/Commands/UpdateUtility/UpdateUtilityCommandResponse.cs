using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Utilities.Commands.UpdateUtility;

public class UpdateUtilityCommandResponse : BaseResponse
{
    public CreateUtilityDto? Utility { get; set; } = default!;
}