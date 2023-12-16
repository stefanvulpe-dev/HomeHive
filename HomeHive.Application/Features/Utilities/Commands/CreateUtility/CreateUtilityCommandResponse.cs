using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Utilities.Commands.CreateUtility;

public class CreateUtilityCommandResponse : BaseResponse
{
    public CreateUtilityDto Utility { get; set; } = null!;
}