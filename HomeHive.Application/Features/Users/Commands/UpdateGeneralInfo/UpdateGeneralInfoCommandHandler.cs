using System.Text.RegularExpressions;
using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Commands.UpdateGeneralInfo;

public class UpdateGeneralInfoCommandHandler(IUserRepository userRepository) : ICommandHandler<UpdateGeneralInfoCommand, UpdateGeneralInfoCommandResponse>
{
    public async Task<UpdateGeneralInfoCommandResponse> Handle(UpdateGeneralInfoCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateGeneralInfoCommandValidator(userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => Regex.Replace(x.PropertyName, ".*\\.", ""), x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            return new UpdateGeneralInfoCommandResponse
            {
                IsSuccess = false,
                Message = "Validation failed.",
                ValidationsErrors = errors
            };
        }
        
        var user = validator.User!;
        
        user.FirstName = request.Data.FirstName;
        user.LastName = request.Data.LastName;
        
        var result = await userRepository.UpdateAsync(user);
        
        if (!result.IsSuccess)
        {
            return new UpdateGeneralInfoCommandResponse
            {
                IsSuccess = false,
                Message = "An error occurred while updating the user."
            };
        }
        
        return new UpdateGeneralInfoCommandResponse
        {
            IsSuccess = true,
            Message = "User updated successfully."
        };
    }
}