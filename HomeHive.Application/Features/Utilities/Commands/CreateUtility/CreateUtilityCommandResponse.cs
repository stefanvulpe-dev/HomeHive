using HomeHive.Application.Responses;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Utilities.Commands.CreateUtility;

public class CreateUtilityCommandResponse : BaseResponse
{
    public CreateUtilityDto Utility { get; set; } = null!;
}