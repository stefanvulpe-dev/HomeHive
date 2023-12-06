using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Commands.DeleteEstateById;

public class DeleteEstateByIdCommandHandler : ICommandHandler<DeleteEstateByIdCommand, DeleteEstateByIdCommandResponse>
{
    private readonly IEstateRepository _estateRepository;

    public DeleteEstateByIdCommandHandler(IEstateRepository estateRepository)
    {
        _estateRepository = estateRepository;
    }

    public async Task<DeleteEstateByIdCommandResponse> Handle(DeleteEstateByIdCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new DeleteEstateByIdCommandValidator(_estateRepository);
        
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return new DeleteEstateByIdCommandResponse
            {
                Success = false,
                ValidationsErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            };
        
        var result = await _estateRepository.DeleteByIdAsync(command.EstateId);
        if (!result.IsSuccess)
            return new DeleteEstateByIdCommandResponse
            {
                Success = false,
                Message = $"Error deleting estate with Id {command.EstateId}"
            };
        
        return new DeleteEstateByIdCommandResponse
        {
            Message = $"Estate with Id {command.EstateId} deleted"
        };
    }
}