using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Users.Commands.CreateEstate;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public class CreateEstateCommandHandler(IEstateRepository repository)
    : ICommandHandler<CreateEstateCommand, CreateEstateCommandResponse>
{
    public async Task<CreateEstateCommandResponse> Handle(CreateEstateCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateEstateCommandValidator();
        var validatorResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validatorResult.IsValid)
            return new CreateEstateCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validatorResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage)
            };

        var result = Estate.Create(command.OwnerId, command.EstateData);

        if (!result.IsSuccess)
            return new CreateEstateCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, string> { { "Estate", result.Message } }
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