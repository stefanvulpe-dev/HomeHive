using HomeHive.Application.Features.Users.Queries.GetUserById;
using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryResponse: BaseResponse
{
    public IReadOnlyList<UserDto>? Users { get; set; }
}