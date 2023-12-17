using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Estates.Commands.CreateEstate;
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
            var validationErrors = validationResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            
            return new UpdateEstateCommandResponse()
            {
                IsSuccess = false,
                Message = "Failed to create contract.",
                ValidationsErrors = validationErrors
            };
        }

        var estateResult = await validator.EstateExist(command, cancellationToken);
        if (!estateResult.IsSuccess)
            return new UpdateEstateCommandResponse
            {
                IsSuccess = false,
                Message = $"Estate {command.EstateId} not found"
            };

        var existingEstate = estateResult.Value;
        existingEstate.UpdateEstate(command.EstateData);

        await _estateRepository.UpdateAsync(existingEstate);

        return new UpdateEstateCommandResponse
        {
            IsSuccess = true,
            Message = "Estate updated successfully",
            Estate = new CreateEstateDto
            {
                Id = existingEstate.Id,
                OwnerId = existingEstate.OwnerId,
                EstateType = existingEstate.EstateType.ToString(),
                EstateCategory = existingEstate.EstateCategory.ToString(),
                Name = existingEstate.Name,
                Location = existingEstate.Location
            }
        };
    }
}