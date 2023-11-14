using HomeHive.Application.Features.Commands.CreateUser;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;
using MediatR;

namespace HomeHive.Application.Features.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
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
        Console.WriteLine(command.UserId);

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
            var existingUserValue = typeof(User).GetProperty(property.Name)?.GetValue(existingUser, null);
            var commandValue = property.GetValue(command.UserData);

            if (commandValue != null)
            {
                Console.WriteLine("1");
                property.SetValue(command.UserData, existingUserValue);
            }
        }

        Console.WriteLine("UserData updated:");
        foreach (var property in userDataProperties)
        {
            var updatedValue = property.GetValue(command.UserData, null);
            Console.WriteLine($"{property.Name}: {updatedValue}");
        }
        var result = User.Create(command.UserId,command.UserData);

        if (!result.IsSuccess)
        {
            return new UpdateUserCommandResponse()
            {
                Success = false,
                Message = "Failed to update user.",
                ValidationsErrors = new List<string> { result.Error }
            };
        }

        existingUser = result.Value;
        await _userRepository.UpdateAsync(existingUser);
        
        return new UpdateUserCommandResponse()
        {
            Message = "User updated successfully.",
            User = new CreateUserDto(existingUser.FirstName, existingUser.LastName, existingUser.Email)
        };
    }
}