using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;

namespace HomeHive.Application.Features.Rooms.Commands.UpdateRoom;

public class UpdateRoomCommandHandler: ICommandHandler<UpdateRoomCommand, UpdateRoomCommandResponse>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IEstateRepository _estateRepository;
    private readonly IEstateRoomRepository _estateRoomRepository;

    public UpdateRoomCommandHandler(IEstateRepository estateRepository, IRoomRepository roomRepository, IEstateRoomRepository estateRoomRepository)
    {
        _estateRepository = estateRepository;
        _roomRepository = roomRepository;
        _estateRoomRepository = estateRoomRepository;
    }

    public async Task<UpdateRoomCommandResponse> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateRoomCommandValidator(_roomRepository, _estateRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new UpdateRoomCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage)
            };
        }
        
        var room = await _roomRepository.FindByIdAsync(request.Id);
        room.Value.Update(request.Data.RoomType);
        await _roomRepository.UpdateAsync(room.Value);
        
        var roomEstate = await _estateRoomRepository.FindBy(request.Data.EstateId, request.Id);
        roomEstate.Value.Update(request.Data.EstateId, request.Id, request.Data.Quantity);
        await _estateRoomRepository.UpdateAsync(roomEstate.Value);
        
        return new UpdateRoomCommandResponse
        {
            IsSuccess = true,
            Room = new CreateRoomDto
            {
                Id = request.Id,
                EstateId = request.Data.EstateId,
                RoomType = request.Data.RoomType,
                Quantity = request.Data.Quantity   
            }
        };

    }
}