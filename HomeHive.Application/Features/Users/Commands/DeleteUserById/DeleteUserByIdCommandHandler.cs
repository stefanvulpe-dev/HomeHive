﻿using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Commands.DeleteUserById;

public class DeleteUserByIdCommandHandler(IUserRepository userRepository)
    : ICommandHandler<DeleteUserByIdCommand, DeleteUserByIdCommandResponse>
{
    public async Task<DeleteUserByIdCommandResponse> Handle(DeleteUserByIdCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new DeleteUserByIdCommandValidator(userRepository);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());

            return new DeleteUserByIdCommandResponse
            {
                IsSuccess = false,
                Message = "Failed to create contract.",
                ValidationsErrors = validationErrors
            };
        }

        var result = await userRepository.DeleteByIdAsync(command.UserId);
        if (!result.IsSuccess)
            return new DeleteUserByIdCommandResponse
            {
                IsSuccess = false,
                Message = $"Message deleting user with Id {command.UserId}"
            };

        return new DeleteUserByIdCommandResponse
        {
            Message = $"User with Id {command.UserId} deleted"
        };
    }
}