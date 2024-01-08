using System.Text.RegularExpressions;
using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

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
            var validationErrors = validationResult.Errors
                .GroupBy(x => Regex.Replace(x.PropertyName, ".*\\.", ""), x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            
            return new UpdateRoomCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validationErrors
            };
        }
        
        var room = await _roomRepository.FindByIdAsync(request.Id);
        room.Value.Update(request.RoomType);
        
        await _roomRepository.UpdateAsync(room.Value);
        
        return new UpdateRoomCommandResponse
        {
            IsSuccess = true,
            Room = new CreateRoomDto
            {
                Id = request.Id,
                RoomType = request.RoomType,
            }
        };

    }
}