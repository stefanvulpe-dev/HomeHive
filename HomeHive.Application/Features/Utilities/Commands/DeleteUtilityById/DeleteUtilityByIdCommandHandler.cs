using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Utilities.Commands.DeleteUtilityById;

public class DeleteUtilityByIdCommandHandler(IUtilityRepository utilityRepository) 
    : ICommandHandler<DeleteUtilityByIdCommand, DeleteUtilityByIdCommandResponse>
{
    public async Task<DeleteUtilityByIdCommandResponse> Handle(DeleteUtilityByIdCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteUtilityByIdCommandValidator(utilityRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            return new DeleteUtilityByIdCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage)
            };
        
        var result = await utilityRepository.DeleteByIdAsync(request.UtilityId);
        
        if (!result.IsSuccess)
            return new DeleteUtilityByIdCommandResponse
            {
                IsSuccess = false,
                Message = $"Error deleting utility with Id {request.UtilityId}"
            };

        return new DeleteUtilityByIdCommandResponse()
        {
            IsSuccess = true,
            Message = $"Utility with Id {request.UtilityId} deleted"
        };
    }
}