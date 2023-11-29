using HomeHive.Application.Features.Users.Commands.CreateUser;
using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandResponse : BaseResponse
{
    public CreateUserDto? User { get; set; }
}