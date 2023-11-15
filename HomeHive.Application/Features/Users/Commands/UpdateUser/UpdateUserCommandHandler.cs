using HomeHive.Application.Abstractions;
using HomeHive.Application.Features.Users.Commands.CreateUser;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

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
        var validator = new UpdateUserCommandValidator();
        var validatorResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validatorResult.IsValid)
        {
            return new UpdateUserCommandResponse()
            {
                Success = false,
                Message = "Failed to update user.",
                ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
            };
        }

        var existingUserResult = await _userRepository.FindByIdAsync(command.UserId);

        if (!existingUserResult.IsSuccess)
        {
            return new UpdateUserCommandResponse()
            {
                Success = false,
                Message = "Failed to update user.",
                ValidationsErrors = new List<string> { "User with this id does not exist." }
            };
        }
        
        var existingUser = existingUserResult.Value;
        
        var userDataProperties = typeof(UserData).GetProperties();

        foreach (var property in userDataProperties)
        {
            var userDataValue = property.GetValue(command.UserData);
    
            if (userDataValue != null)
            {
                var existingUserProperty = typeof(User).GetProperty(property.Name);
                if (existingUserProperty != null)
                {
                    existingUserProperty.SetValue(existingUser, userDataValue);
                }
            }
        }
        await _userRepository.UpdateAsync(existingUser);
        
        return new UpdateUserCommandResponse()
        {
            Message = "User updated successfully.",
            User = new CreateUserDto(existingUser.Id, existingUser.FirstName, existingUser.LastName, existingUser.Email)
        };
    }
}