using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public class CreateEstateCommandHandler(IBlobStorageService blobStorageService, IEstateRepository repository, IUtilityRepository utilityRepository,
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
        
        var estateAvatarName = Guid.NewGuid().ToString();
        
        var estateAvatarFile = command.CreateEstateFormData.EstateAvatar!.OpenReadStream();
        
        var uploadResult = await blobStorageService.UploadBlobAsync(estateAvatarName, estateAvatarFile, cancellationToken);
        
        if (!uploadResult)
            return new CreateEstateCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, List<string>> { { "EstateAvatar",  new List<string> {"Failed to upload estate avatar."} } }
            };

        var createEstateData = new EstateData(
            command.CreateEstateFormData.EstateType,
            command.CreateEstateFormData.EstateCategory,
            command.CreateEstateFormData.Name,
            command.CreateEstateFormData.Location,
            command.CreateEstateFormData.Price,
            command.CreateEstateFormData.TotalArea,
            command.CreateEstateFormData.Utilities,
            command.CreateEstateFormData.Rooms,
            command.CreateEstateFormData.Description,
            estateAvatarName
        );
        
        var createResult = Estate.Create(command.OwnerId, validator.Utilities!, createEstateData);
        
        if (!createResult.IsSuccess)
            return new CreateEstateCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, List<string>> { { "Estate",  createResult.ValidationErrors!.Select(er => er.Value).ToList() } }
            };
        
        var estate = createResult.Value;
        
        var addResult = await repository.AddAsync(estate);
        
        if (!addResult.IsSuccess)
            return new CreateEstateCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, List<string>> { { "Estate",  new List<string> {addResult.Message} } }
            };
        
        var estateRooms = new List<EstateRoom>();
        
        foreach (var estateRoom in validator.Rooms!.Select(pair => EstateRoom.Create(estate.Id, pair.Key.Id, pair.Value)))
        {
            if (!estateRoom.IsSuccess)
                return new CreateEstateCommandResponse
                {
                    IsSuccess = false,
                    ValidationsErrors = new Dictionary<string, List<string>> { { "Rooms",  estateRoom.ValidationErrors!.Select(er => er.Value).ToList() } }
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