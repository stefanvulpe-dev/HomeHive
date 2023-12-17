using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Utilities.Commands.CreateUtility;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Utilities.Commands.UpdateUtility;

public class UpdateUtilityCommandHandler(IUtilityRepository utilityRepository)
    : ICommandHandler<UpdateUtilityCommand, UpdateUtilityCommandResponse>
{
    public async Task<UpdateUtilityCommandResponse> Handle(UpdateUtilityCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateUtilityCommandValidator(utilityRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return new UpdateUtilityCommandResponse
            {
                IsSuccess = false,
                Message = "Error updating utility",
                ValidationsErrors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage)
            };
        
        var utilityResult = await validator.UtilityExists(request.UtilityId, cancellationToken);
        if (!utilityResult.IsSuccess)
            return new UpdateUtilityCommandResponse
            {
                IsSuccess = false,
                Message = $"Utility {request.UtilityId} not found"
            };
        
        var existingUtility = utilityResult.Value;
        existingUtility.Update(request.UtilityName);
        
        await utilityRepository.UpdateAsync(existingUtility);
        
        return new UpdateUtilityCommandResponse
        {
            IsSuccess = true,
            Message = "Utility updated successfully",
            Utility = new CreateUtilityDto
            {
                Id = existingUtility.Id,
                UtilityName = existingUtility.UtilityName
            }
        };
    }
}