using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Estates.Commands.CreateEstate;
using HomeHive.Application.Features.Users.Commands.CreateEstate;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstate;

public class UpdateEstateCommandHandler : ICommandHandler<UpdateEstateCommand, UpdateEstateCommandResponse>
{
    private readonly IEstateRepository _estateRepository;

    public UpdateEstateCommandHandler(IEstateRepository estateRepository)
    {
        _estateRepository = estateRepository;
    }

    public async Task<UpdateEstateCommandResponse> Handle(UpdateEstateCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateEstateCommandValidator(_estateRepository);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new UpdateEstateCommandResponse()
            {
                Success = false,
                Message = "Error updating estate",
                ValidationsErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            };
        }

        var estateResult = await validator.EstateExist(command, cancellationToken);
        if (!estateResult.IsSuccess)
        {
            return new UpdateEstateCommandResponse()
            {
                Success = false,
                Message = $"Estate {command.EstateId} not found",
                ValidationsErrors = new List<string> { estateResult.Error }
            };
        }

        var existingEstate = estateResult.Value;
        existingEstate.UpdateEstate(command.EstateData);

        await _estateRepository.UpdateAsync(existingEstate);

        return new UpdateEstateCommandResponse()
        {
            Success = true,
            Message = "Estate updated successfully",
            Estate = new CreateEstateDto(existingEstate.Id, existingEstate.OwnerId,
                existingEstate.EstateType.ToString(), existingEstate.EstateCategory.ToString(), existingEstate.Name,
                existingEstate.Location)
        };
    }
}