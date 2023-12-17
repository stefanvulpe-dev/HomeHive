using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Users.Queries.GetProfilePicture;

public class GetProfilePictureQueryResponse : BaseResponse
{
    public Stream? Content { get; set; }
}