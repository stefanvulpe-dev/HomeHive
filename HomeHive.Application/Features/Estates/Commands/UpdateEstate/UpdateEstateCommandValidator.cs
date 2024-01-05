using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstate;

public class UpdateEstateCommandValidator : AbstractValidator<UpdateEstateCommand>
{
    private readonly IEstateRepository _estateRepository;
    private readonly IUtilityRepository _utilityRepository;
    private readonly IRoomRepository _roomRepository;
    public List<Utility>? Utilities { get; private set; } = new ();
    public Dictionary<Room, int>? Rooms { get; private set; } = new ();

    public UpdateEstateCommandValidator(IEstateRepository estateRepository, IUtilityRepository utilityRepository, IRoomRepository roomRepository)
    {
        _estateRepository = estateRepository;
        _utilityRepository = utilityRepository;
        _roomRepository = roomRepository;

        RuleFor(v => v.EstateId)
            .NotEmpty().WithMessage("EstateId is required.")
            .MustAsync(EstateExist).WithMessage("Estate does not exist.");
        RuleFor(p => p.EstateData)
            .NotNull().WithMessage("EstateData is required.")
            .Custom((estateData, context) =>
            {
                if (string.IsNullOrWhiteSpace(estateData.EstateType) &&
                    string.IsNullOrWhiteSpace(estateData.EstateCategory) &&
                    string.IsNullOrWhiteSpace(estateData.Name) &&
                    string.IsNullOrWhiteSpace(estateData.Location) &&
                    estateData.Price <= 0 &&
                    string.IsNullOrWhiteSpace(estateData.TotalArea) &&
                    (estateData.Utilities == null || estateData.Utilities.Count == 0) &&
                    (estateData.Rooms == null || estateData.Rooms.Count == 0) &&
                    string.IsNullOrWhiteSpace(estateData.Description) &&
                    string.IsNullOrWhiteSpace(estateData.EstateAvatar))
                    context.AddFailure("At least one field in EstateData must be provided.");
            });
        RuleFor(v => v.EstateData.Utilities)
            .MustAsync(ValidateUtilitiesExistence).WithMessage("Utility does not exist.")
            .When(v => v.EstateData.Utilities != null && v.EstateData.Utilities.Count > 0);
        RuleFor(v => v.EstateData.Rooms)
            .MustAsync(ValidateRooms).WithMessage("Room does not exist.")
            .When(v => v.EstateData.Rooms != null && v.EstateData.Rooms.Count > 0);
    }

    private async Task<bool> EstateExist(Guid estateId, CancellationToken arg2)
    {
        if (estateId == Guid.Empty)
            return false;
        
        var result = await _estateRepository.FindByIdAsync(estateId);
        return result.IsSuccess;
    }
    
    private async Task<bool> ValidateUtilitiesExistence(List<string>? utilities, CancellationToken cancellationToken)
    {
        var utilitiesResult = await _utilityRepository.GetAllAsync();
        var utilitiesNames = utilitiesResult.Value.Select(u => u.UtilityType.ToString()).ToList();
        Utilities = utilitiesResult.Value.Where(u => utilities!.Contains(u.UtilityType.ToString())).ToList();
        if (utilities!.Any(utility => !utilitiesNames.Contains(utility))) return false;
        Utilities = utilitiesResult.Value.Where(u => utilities!.Contains(u.UtilityType.ToString())).ToList();
        return true;
    }
    
    private async Task<bool> ValidateRooms(Dictionary<string, int>? rooms, CancellationToken cancellationToken)
    {
        if (rooms == null)
            return true;
        
        var roomsResult = await _roomRepository.GetAllAsync();
        var roomsList = roomsResult.Value.Select(r => r.RoomType.ToString()).ToList();

        foreach (var room in rooms.Keys)
        {
            if (!roomsList.Contains(room))
                return false;
        }
        
        var roomsQuantity = rooms.Values.ToList();
        foreach (var quantity in roomsQuantity)
        {
            if (quantity <= 0)
                return false;
        }
        
        Rooms = roomsResult.Value
            .Where(r => rooms.Keys.Contains(r.RoomType.ToString()!))
            .ToDictionary(r => r, r => rooms[r.RoomType.ToString()!]);
        return true;
    }
}