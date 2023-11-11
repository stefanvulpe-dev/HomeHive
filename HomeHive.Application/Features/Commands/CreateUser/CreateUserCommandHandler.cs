using HomeHive.Application.Abstractions;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Commands.CreateUser;

public class CreateUserCommandHandler: ICommandHandler<CreateUserCommand, CreateUserCommandResponse>
{
    private readonly IUserRepository _userRepository;
    
    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<CreateUserCommandResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateUserCommandValidator();
        var validatorResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (!validatorResult.IsValid)
        {
            return new CreateUserCommandResponse
            {
                Success = false,
                Message = "Failed to create user.",
                ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
            };
        }
        
        var result = User.Create(command.UserData);
        
        if (!result.IsSuccess)
        {
            return new CreateUserCommandResponse
            {
                Success = false,
                Message = "Failed to create user.",
                ValidationsErrors = new List<string> { result.Error }
            };
        }
        
        var user = result.Value;
        await _userRepository.AddAsync(user);
        
        return new CreateUserCommandResponse
        {
            Message = "User created successfully.",
            User = new CreateUserDto(user.FirstName, user.LastName, user.Email)
        };
    }
}