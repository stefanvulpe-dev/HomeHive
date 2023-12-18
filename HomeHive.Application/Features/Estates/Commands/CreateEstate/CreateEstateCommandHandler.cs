using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public class CreateEstateCommandHandler(IEstateRepository repository, IUtilityRepository utilityRepository,
    IRoomRepository roomRepository, IEstateRoomRepository estateRoomRepository)
    : ICommandHandler<CreateEstateCommand, CreateEstateCommandResponse>
{
    public async Task<CreateEstateCommandResponse> Handle(CreateEstateCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateEstateCommandValidator(utilityRepository, roomRepository);
        var validatorResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validatorResult.IsValid)
        {
            var validationErrors = validatorResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());

            return new CreateEstateCommandResponse
            {
                IsSuccess = false,
                Message = "Failed to create estate.",
                ValidationsErrors = validationErrors
            };
        }

        var result = Estate.Create(command.OwnerId, validator.Utilities!, command.EstateData);
        var estate = result.Value;
        await repository.AddAsync(estate);
        
        if (!result.IsSuccess)
            return new CreateEstateCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, List<string>> { { "Estate", [result.Message] } }
            };
        
        var estateRooms = new List<EstateRoom>();
        
        foreach (var pair in validator.Rooms!)
        {
            var estateRoom = EstateRoom.Create(estate.Id, pair.Key.Id, pair.Value);
            if (!estateRoom.IsSuccess)
                return new CreateEstateCommandResponse
                {
                    IsSuccess = false,
                    ValidationsErrors = new Dictionary<string, List<string>> { { "EstateRoom", new List<string> { result.Error } } }
                };
        
            await estateRoomRepository.AddAsync(estateRoom.Value);
            estateRooms.Add(estateRoom.Value);
        }
        
        estate.AddEstateRooms(estateRooms);

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