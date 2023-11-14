using HomeHive.Application.Abstractions;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Commands.UpdateUser;

public record UpdateUserCommand : ICommand<UpdateUserCommandResponse>
{
    public Guid UserId { get; set; }
    public UserData UserData { get; set; }
}