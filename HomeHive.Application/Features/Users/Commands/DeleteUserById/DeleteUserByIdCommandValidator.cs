using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Commands.DeleteUserById;

public class DeleteUserByIdCommandValidator : AbstractValidator<DeleteUserByIdCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserByIdCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MustAsync(Exists).WithMessage("User with Id {PropertyValue} does not exist.");
    }

    private async Task<bool> Exists(Guid arg1, CancellationToken arg2)
    {
        var result = await _userRepository.FindByIdAsync(arg1);
        return result.IsSuccess;
    }
}