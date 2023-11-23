using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;
    
    public UpdateUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        RuleSet("UpdateUser", () =>
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("UserId is required.");
            RuleFor(p => p.UserData)
                .NotNull().WithMessage("UserData is required.")
                .Custom((userData, context) =>
                {
                    if (string.IsNullOrWhiteSpace(userData.FirstName) &&
                        string.IsNullOrWhiteSpace(userData.LastName) &&
                        string.IsNullOrWhiteSpace(userData.Email) &&
                        string.IsNullOrWhiteSpace(userData.Password) &&
                        string.IsNullOrWhiteSpace(userData.PhoneNumber) &&
                        string.IsNullOrWhiteSpace(userData.ProfilePicture))
                    {
                        context.AddFailure("At least one field in UserData must be provided.");
                    }
                });
        });
    }

    public async Task<Result<User>> ValidateUserExistence(UpdateUserCommand command, CancellationToken arg2)
    {
        var userResult = await _userRepository.FindByIdAsync(command.UserId);
        return userResult.IsSuccess ? Result<User>.Success(userResult.Value) : Result<User>.Failure("User with this id does not exist.");
    }
}