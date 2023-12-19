using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;

namespace HomeHive.Application.Features.Rooms.Commands.UpdateRoom;

public class UpdateRoomCommandValidator: AbstractValidator<UpdateRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IEstateRepository _estateRepository;

    public UpdateRoomCommandValidator(IRoomRepository roomRepository, IEstateRepository estateRepository)
    {
        _roomRepository = roomRepository;
        _estateRepository = estateRepository;

        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Id is required.")
            .MustAsync(RoomExists).WithMessage("Room does not exist.");

        RuleFor(r => r.RoomType)
            .NotNull().WithMessage("Room Type is required.")
            .MustAsync(RoomTypeUnique).WithMessage("Room type already exists.")
            .Must((r, roomType) => Enum.TryParse(typeof(RoomType), roomType.ToString(), out _))
            .WithMessage("Invalid Room Type.");
        
    }
    
    private async Task<bool> RoomExists(Guid roomId, CancellationToken cancellationToken)
    {
        var roomResult = await _roomRepository.FindByIdAsync(roomId);
            
        return roomResult.IsSuccess;
    }
    
    private async Task<bool> RoomTypeUnique(string roomType, CancellationToken cancellationToken)
    {
        var roomsResult = await _roomRepository.GetAllAsync();
        if (!roomsResult.IsSuccess)
            return false;
        
        var rooms = roomsResult.Value.Select(r => r.RoomType).ToList();
        return !roomsResult.Value.Any(r => r.RoomType.ToString() == roomType);
    }
}