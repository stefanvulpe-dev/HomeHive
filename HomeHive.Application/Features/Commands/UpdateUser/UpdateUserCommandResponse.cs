using HomeHive.Application.Features.Commands.CreateUser;
using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Commands.UpdateUser;

public class UpdateUserCommandResponse : BaseResponse
{
    public UpdateUserCommandResponse(): base()
    {
    }
    
    public CreateUserDto? User { get; set; }
}
