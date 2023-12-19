using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Utilities.Commands.CreateUtility;

public class CreateUtilityCommandHandler(IUtilityRepository utilityRepository)
    : ICommandHandler<CreateUtilityCommand, CreateUtilityCommandResponse>
{
    public async Task<CreateUtilityCommandResponse> Handle(CreateUtilityCommand request,
        CancellationToken cancellationToken)
    {
        var validator = new CreateUtilityCommandValidator(utilityRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());

            return new CreateUtilityCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validationErrors
            };
        }

        var utilityResult = Utility.Create(request.UtilityName);

        if (!utilityResult.IsSuccess)
            return new CreateUtilityCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, List<string>> { { "Utility", new List<string> { utilityResult.Message } } }
            };

        var utility = utilityResult.Value;
        await utilityRepository.AddAsync(utility);

        return new CreateUtilityCommandResponse
        {
            IsSuccess = true,
            Utility = new CreateUtilityDto
            {
                Id = utility.Id,
                UtilityName = utility.UtilityName
            }
        };
    }
}