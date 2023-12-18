using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Features.Rooms.Commands;
using HomeHive.Application.Features.Rooms.Commands.CreateRoom;
using HomeHive.Application.Features.Rooms.Commands.DeleteRoomById;
using HomeHive.Application.Features.Rooms.Commands.UpdateRoom;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class RoomsController(ICurrentUserService currentUserService,
    ITokenCacheService tokenCacheService,
    ILogger<EstatesController> logger): ApiBaseController
{
 
    [Authorize(Roles = "User, Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] RoomData data)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        
        var result = await Mediator.Send(new CreateRoomCommand(data));
        
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }
        return CreatedAtAction(nameof(Create), result);
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpPut("{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid roomId, [FromBody] RoomData data)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        
        var result = await Mediator.Send(new UpdateRoomCommand(roomId, data));
        
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpDelete("{roomId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid roomId)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        
        var result = await Mediator.Send(new DeleteRoomByIdCommand(roomId));
        
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }
        return NoContent();
    }
    
}