using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Commands.DeleteUser;

public class DeleteUserCommandResponse: BaseResponse
{
    public DeleteUserCommandResponse()
    {
    }

    public string? Email { get; set; }
}