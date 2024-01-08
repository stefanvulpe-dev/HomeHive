using System.Text.RegularExpressions;
using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Rooms.Commands.DeleteRoomById;

public class DeleteRoomByIdCommandHandler(IRoomRepository roomRepository): 
    ICommandHandler<DeleteRoomByIdCommand, DeleteRoomByIdCommandResponse>
{
    public async Task<DeleteRoomByIdCommandResponse> Handle(DeleteRoomByIdCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteRoomByIdCommandValidator(roomRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .GroupBy(x => Regex.Replace(x.PropertyName, ".*\\.", ""), x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            
            return new DeleteRoomByIdCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validationErrors
            };
        }
        
        var result = await roomRepository.DeleteByIdAsync(request.RoomId);
        if (!result.IsSuccess)
            return new DeleteRoomByIdCommandResponse
            {
                IsSuccess = false,
                Message = $"Error deleting room with Id {request.RoomId}"
            };

        return new DeleteRoomByIdCommandResponse
        {
            IsSuccess = true,
            Message = $"Room with Id {request.RoomId} deleted"
        };
    }
}