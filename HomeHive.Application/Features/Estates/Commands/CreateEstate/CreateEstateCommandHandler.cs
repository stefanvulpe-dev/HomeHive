using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public class CreateEstateCommandHandler(IEstateRepository repository, IUtilityRepository utilityRepository)
    : ICommandHandler<CreateEstateCommand, CreateEstateCommandResponse>
{
    public async Task<CreateEstateCommandResponse> Handle(CreateEstateCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateEstateCommandValidator(utilityRepository);
        var validatorResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validatorResult.IsValid)
        {
            var validationErrors = validatorResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());

            return new CreateEstateCommandResponse
            {
                IsSuccess = false,
                Message = "Failed to create contract.",
                ValidationsErrors = validationErrors
            };
        }

        var result = Estate.Create(command.OwnerId, validator.Utilities!, command.EstateData);

        if (!result.IsSuccess)
            return new CreateEstateCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, List<string>> { { "Estate", [result.Message] } }
            };
        var estate = result.Value;

        await repository.AddAsync(estate);

        return new CreateEstateCommandResponse
        {
            IsSuccess = true,
            Estate = new CreateEstateDto
            {
                Id = estate.Id,
                OwnerId = estate.OwnerId,
                EstateType = estate.EstateType.ToString(),
                EstateCategory = estate.EstateCategory.ToString(),
                Name = estate.Name,
                Location = estate.Location
            }
        };
    }
}