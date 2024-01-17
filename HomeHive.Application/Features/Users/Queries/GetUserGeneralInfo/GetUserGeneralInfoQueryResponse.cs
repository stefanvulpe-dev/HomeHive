using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Users.Queries.GetUserGeneralInfo;

public class GetUserGeneralInfoQueryResponse : BaseResponse
{
    public UserDto? User { get; set; }
}