using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstateAvatar;

public class UpdateEstateAvatarCommandResponse : BaseResponse
{
    public string EstateAvatar { get; set; }
}