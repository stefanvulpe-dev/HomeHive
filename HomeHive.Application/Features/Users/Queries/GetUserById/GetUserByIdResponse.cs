using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdResponse : BaseResponse
{
    public UserDto? User { get; set; }
}