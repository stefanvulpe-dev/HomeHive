using FluentValidation;
using HomeHive.Application.Persistence;

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

        RuleFor(r => r.Data)
            .NotNull().WithMessage("Room data is required.")
            .Custom((roomData, context) =>
            {
                if (string.IsNullOrWhiteSpace(roomData.RoomType)
                    && roomData.Quantity <= 0
                    && roomData.EstateId == Guid.Empty)
                    context.AddFailure("At least one field in RoomData must be provided.");
            });
        RuleFor(r => r.Data.EstateId)
            .MustAsync(EstateExists).WithMessage("Estate does not exist.");
        

    }
    
    private async Task<bool> EstateExists(Guid estateId, CancellationToken cancellationToken)
    {
        var estateResult = await _estateRepository.FindByIdAsync(estateId);
            
        return estateResult.IsSuccess;
    }
    
    private async Task<bool> RoomExists(Guid roomId, CancellationToken cancellationToken)
    {
        var roomResult = await _roomRepository.FindByIdAsync(roomId);
            
        return roomResult.IsSuccess;
    }
}