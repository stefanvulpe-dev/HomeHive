using HomeHive.Application.Abstractions;
using HomeHive.Application.Features.Users.Commands.CreateUser;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserCommandResponse>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateUserCommandValidator(_userRepository);
        var validatorResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validatorResult.IsValid)
            return new UpdateUserCommandResponse
            {
                Success = false,
                Message = "Failed to update user.",
                ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
            };

        var userResult = await validator.ValidateUserExistence(command, cancellationToken);
        if (!userResult.IsSuccess)
            return new UpdateUserCommandResponse
            {
                Success = false,
                Message = "Failed to update user.",
                ValidationsErrors = new List<string> { userResult.Error }
            };

        var existingUser = userResult.Value;
        existingUser.UpdateUser(command.UserData);

        await _userRepository.UpdateAsync(existingUser);

        return new UpdateUserCommandResponse
        {
            Message = "User updated successfully.",
            User = new CreateUserDto(existingUser.Id, existingUser.FirstName, existingUser.LastName, existingUser.Email)
        };
    }
}