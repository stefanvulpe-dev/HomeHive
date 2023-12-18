using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;

namespace HomeHive.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandValidator: AbstractValidator<CreateRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    
    public CreateRoomCommandValidator(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
        RuleFor(p => p.RoomType)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MustAsync(RoomTypeUnique).WithMessage("Room type already exists.");
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