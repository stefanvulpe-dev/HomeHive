using System.Text.RegularExpressions;
using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Estates.Commands.CreateEstate;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstate;

public class UpdateEstateCommandHandler : ICommandHandler<UpdateEstateCommand, UpdateEstateCommandResponse>
{
    private readonly IEstateRepository _estateRepository;
    private readonly IUtilityRepository _utilityRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEstateRoomRepository _estateRoomRepository;

    public UpdateEstateCommandHandler(IEstateRepository estateRepository, IUtilityRepository utilityRepository,
        IRoomRepository roomRepository, IEstateRoomRepository estateRoomRepository)
    {
        _estateRepository = estateRepository;
        _utilityRepository = utilityRepository;
        _roomRepository = roomRepository;
        _estateRoomRepository = estateRoomRepository;
    }

    public async Task<UpdateEstateCommandResponse> Handle(UpdateEstateCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateEstateCommandValidator(_estateRepository, _utilityRepository, _roomRepository);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .GroupBy(x => Regex.Replace(x.PropertyName, ".*\\.", ""), x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());

            return new UpdateEstateCommandResponse
            {
                IsSuccess = false,
                Message = "Failed to update estate.",
                ValidationsErrors = validationErrors
            };
        }

        var estateResult = await _estateRepository.FindByIdAsync(command.EstateId);
        if (!estateResult.IsSuccess)
            return new UpdateEstateCommandResponse
            {
                IsSuccess = false,
                Message = $"Estate {command.EstateId} not found"
            };
        
        var existingEstate = estateResult.Value;
        
        var estateRooms = new List<EstateRoom>();
        
        if (validator.Rooms!.Count > 0)
        {
            foreach (var pair in validator.Rooms)
            {
                var estateRoomResult = await _estateRoomRepository.FindBy(existingEstate.Id, pair.Key.Id);
                if (estateRoomResult.IsSuccess)
                {
                    estateRoomResult.Value.Update(existingEstate.Id, pair.Key.Id, pair.Value);
                    await _estateRoomRepository.UpdateAsync(estateRoomResult.Value);
                    estateRooms.Add(estateRoomResult.Value);
                }
                else
                {
                    var estateRoom = EstateRoom.Create(existingEstate.Id, pair.Key.Id, pair.Value);
                    if (!estateRoom.IsSuccess)
                        return new UpdateEstateCommandResponse
                        {
                            IsSuccess = false,
                            ValidationsErrors = new Dictionary<string, List<string>> { { "EstateRoom",  estateRoom.ValidationErrors!.Select(er => er.Value).ToList() } }
                        };
            
                    await _estateRoomRepository.AddAsync(estateRoomResult.Value);
                    estateRooms.Add(estateRoomResult.Value);
                }
            }
        }
        
        existingEstate.Update(validator.Utilities!, estateRooms, command.EstateData);

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