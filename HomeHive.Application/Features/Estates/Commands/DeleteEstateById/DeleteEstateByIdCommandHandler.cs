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
            return new DeleteEstateByIdCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage)
            };

        var result = await estateRepository.DeleteByIdAsync(command.EstateId);
        if (!result.IsSuccess)
            return new DeleteEstateByIdCommandResponse
            {
                IsSuccess = false,
                Message = $"Message deleting estate with Id {command.EstateId}"
            };

        return new DeleteEstateByIdCommandResponse
        {
            Message = $"Estate with Id {command.EstateId} deleted"
        };
    }
}