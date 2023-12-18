using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Commands.DeleteEstateById;

public class DeleteEstateByIdCommandHandler(IEstateRepository estateRepository)
    : ICommandHandler<DeleteEstateByIdCommand, DeleteEstateByIdCommandResponse>
{
    public async Task<DeleteEstateByIdCommandResponse> Handle(DeleteEstateByIdCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new DeleteEstateByIdCommandValidator(estateRepository);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            
            return new DeleteEstateByIdCommandResponse()
            {
                IsSuccess = false,
                Message = "Failed to create contract.",
                ValidationsErrors = validationErrors
            };
        }

        var result = await estateRepository.DeleteByIdAsync(command.EstateId);
        if (!result.IsSuccess)
            return new DeleteEstateByIdCommandResponse
            {
                IsSuccess = false,
                Message = $"Error deleting estate with Id {command.EstateId}"
            };

        return new DeleteEstateByIdCommandResponse
        {
            Message = $"Estate with Id {command.EstateId} deleted"
        };
    }
}