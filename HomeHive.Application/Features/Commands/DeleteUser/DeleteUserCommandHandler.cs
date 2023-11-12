using HomeHive.Application.Abstractions;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Commands.DeleteUser;

public class DeleteUserCommandHandler: ICommandHandler<DeleteUserCommand, DeleteUserCommandResponse>
{
    private readonly IUserRepository _userRepository;
    
    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(command.Email);
        if (existingUser == null)
        {
            return new DeleteUserCommandResponse
            {
                Success = false,
                Message = "Failed to delete user.",
                ValidationsErrors = new List<string> { "User with this email does not exist." }
            };
        }
        
        await _userRepository.DeleteByEmailAsync(command.Email);

        return new DeleteUserCommandResponse()
        {
            Message = "User deleted successfully."
        };
    }
    
}