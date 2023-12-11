using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Commands.DeleteUserById;

public class DeleteUserByIdCommandHandler : ICommandHandler<DeleteUserByIdCommand, DeleteUserByIdCommandResponse>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserByIdCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<DeleteUserByIdCommandResponse> Handle(DeleteUserByIdCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new DeleteUserByIdCommandValidator(_userRepository);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return new DeleteUserByIdCommandResponse
            {
                Success = false,
                ValidationsErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            };

        var result = await _userRepository.DeleteByIdAsync(command.UserId);
        if (!result.IsSuccess)
            return new DeleteUserByIdCommandResponse
            {
                Success = false,
                Message = $"Error deleting user with Id {command.UserId}"
            };

        return new DeleteUserByIdCommandResponse
        {
            Message = $"User with Id {command.UserId} deleted"
        };
    }
}