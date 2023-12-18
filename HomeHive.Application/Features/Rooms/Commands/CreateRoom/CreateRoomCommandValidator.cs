using FluentValidation;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandValidator: AbstractValidator<CreateRoomCommand>
{
    private readonly IEstateRepository _estateRepository;
    
    public CreateRoomCommandValidator(IEstateRepository estateRepository)
    {
        _estateRepository = estateRepository;
        RuleFor(p => p.Room.RoomType)
            .NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(p => p.Room.EstateId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MustAsync(EstateExists).WithMessage("Estate does not exist.");
        RuleFor(p => p.Room.Quantity)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0);
    }

    private async Task<bool> EstateExists(Guid estateId, CancellationToken cancellationToken)
    {
        var estateResult = await _estateRepository.FindByIdAsync(estateId);
            
        return estateResult.IsSuccess;
    }
}