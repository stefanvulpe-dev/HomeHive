using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler(IRoomRepository roomRepository, IEstateRepository estateRepository, IEstateRoomRepository estateRoomRepository): ICommandHandler<CreateRoomCommand, CreateRoomCommandResponse>
{
    public async Task<CreateRoomCommandResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateRoomCommandValidator(estateRepository);
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            
            return new CreateRoomCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, string>{{validatorResult.Errors[0].PropertyName, validatorResult.Errors[0].ErrorMessage}}
            };
        }
        
        var result = Room.Create(request.Room.RoomType);
        if (!result.IsSuccess)
            return new CreateRoomCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, string> {{"Room", result.Error}}
            };
        
        var room = await roomRepository.AddAsync(result.Value);
        if (!room.IsSuccess)
            return new CreateRoomCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, string> {{"Room", room.Error}}
            };
        
        var estateRoom = EstateRoom.Create(request.Room.EstateId, room.Value.Id, request.Room.Quantity);
        if (!estateRoom.IsSuccess)
            return new CreateRoomCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, string> {{"EstateRoom", estateRoom.Error}}
            };
        
        await estateRoomRepository.AddAsync(estateRoom.Value);
        
        return new CreateRoomCommandResponse
        {
            IsSuccess = true,
            Room= new CreateRoomDto
            {
                Id = room.Value.Id,
                EstateId = request.Room.EstateId,
                RoomType = room.Value.RoomType.ToString(),
                Quantity = request.Room.Quantity
            }
        };
    }
}