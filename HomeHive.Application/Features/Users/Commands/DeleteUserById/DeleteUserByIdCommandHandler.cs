using HomeHive.Application.Abstractions;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Commands.DeleteUserById;

public class DeleteUserByIdCommandHandler: ICommandHandler<DeleteUserByIdCommand, DeleteUserByIdCommandResponse>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserByIdCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<DeleteUserByIdCommandResponse> Handle(DeleteUserByIdCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.FindByIdAsync(command.Id);
        if (!existingUser.IsSuccess)
        {
            return new DeleteUserByIdCommandResponse()
            {
                Success = false,
                Message = $"User with Id {command.Id} not found"
            };
        }
        
        var result = await _userRepository.DeleteByIdAsync(existingUser.Value.Id);
        if (!result.IsSuccess)
        {
            return new DeleteUserByIdCommandResponse()
            {
                Success = false,
                Message = $"Error deleting user with Id {command.Id}"
            };
        }
        
        return new DeleteUserByIdCommandResponse()
        {
            Message = $"User with Id {command.Id} deleted"
        };
    }
}