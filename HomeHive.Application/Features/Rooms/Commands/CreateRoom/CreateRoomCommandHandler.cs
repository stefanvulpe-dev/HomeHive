using System.Text.RegularExpressions;
using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler(IRoomRepository roomRepository): ICommandHandler<CreateRoomCommand, CreateRoomCommandResponse>
{
    public async Task<CreateRoomCommandResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateRoomCommandValidator(roomRepository);
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            var validationErrors = validatorResult.Errors
                .GroupBy(x => Regex.Replace(x.PropertyName, ".*\\.", ""), x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            
            return new CreateRoomCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validationErrors
            };
        }
        
        var result = Room.Create(request.RoomType);
        if (!result.IsSuccess)
            return new CreateRoomCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, List<string>> { { "Room",  [ result.Message ] } }
            };
        
        var room = await roomRepository.AddAsync(result.Value);
        if (!room.IsSuccess)
            return new CreateRoomCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = new Dictionary<string, List<string>> { { "Room",  result.ValidationErrors!.Select(er => er.Value).ToList() } }
            };
        
        return new CreateRoomCommandResponse
        {
            IsSuccess = true,
            Room= new CreateRoomDto
            {
                Id = room.Value.Id,
                RoomType = room.Value.RoomType.ToString(),
            }
        };
    }
}