using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Users.Commands.CreateEstate;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public class CreateEstateCommandHandler : ICommandHandler<CreateEstateCommand, CreateEstateCommandResponse>
{
    private readonly IEstateRepository _repository;

    public CreateEstateCommandHandler(IEstateRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateEstateCommandResponse> Handle(CreateEstateCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateEstateCommandValidator();
        var validatorResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validatorResult.IsValid)
            return new CreateEstateCommandResponse
            {
                Success = false,
                ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
            };
        
        var result = Estate.Create(command.OwnerId, command.EstateData);

        if (!result.IsSuccess)
            return new CreateEstateCommandResponse
            {
                Success = false,
                ValidationsErrors = new List<string> { result.Error }
            };
        var estate = result.Value;

        await _repository.AddAsync(estate);

        return new CreateEstateCommandResponse
        {
            Success = true,
            Estate = new CreateEstateDto(estate.Id, estate.OwnerId, estate.EstateType.ToString(),
                estate.EstateCategory.ToString(), estate.Name, estate.Location)
        };
    }
}