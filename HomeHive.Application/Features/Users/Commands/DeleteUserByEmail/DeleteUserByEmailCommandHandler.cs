using HomeHive.Application.Abstractions;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Commands.DeleteUserByEmail;

public class DeleteUserByEmailCommandHandler: ICommandHandler<DeleteUserByEmailCommand, DeleteUserByEmailCommandResponse>
{
    private readonly IUserRepository _userRepository;
    
    public DeleteUserByEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<DeleteUserByEmailCommandResponse> Handle(DeleteUserByEmailCommand byEmailCommand, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(byEmailCommand.Email);
        if (existingUser == null)
        {
            return new DeleteUserByEmailCommandResponse
            {
                Success = false,
                Message = "Failed to delete user.",
                ValidationsErrors = new List<string> { "User with this email does not exist." }
            };
        }
        
        await _userRepository.DeleteByEmailAsync(byEmailCommand.Email);

        return new DeleteUserByEmailCommandResponse()
        {
            Message = "User deleted successfully."
        };
    }
    
}