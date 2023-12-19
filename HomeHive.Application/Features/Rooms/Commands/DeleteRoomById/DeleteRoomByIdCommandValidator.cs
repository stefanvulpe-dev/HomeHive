using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;

namespace HomeHive.Application.Features.Rooms.Commands.DeleteRoomById;

public class DeleteRoomByIdCommandValidator: AbstractValidator<DeleteRoomByIdCommand>
{
    private readonly IRoomRepository _roomRepository;

    public DeleteRoomByIdCommandValidator(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
        RuleFor(p => p.RoomId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MustAsync(RoomExists).WithMessage("Room does not exist.");
    }
    
    private async Task<bool> RoomExists(Guid roomId, CancellationToken cancellationToken)
    {
        var roomResult = await _roomRepository.FindByIdAsync(roomId);
            
        return roomResult.IsSuccess;
    }
    
}