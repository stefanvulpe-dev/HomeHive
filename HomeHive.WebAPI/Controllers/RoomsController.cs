using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Features.Rooms.Commands;
using HomeHive.Application.Features.Rooms.Commands.CreateRoom;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class RoomsController(ICurrentUserService currentUserService,
    ITokenCacheService tokenCacheService,
    ILogger<EstatesController> logger): ApiBaseController
{
 
    // [Authorize(Roles = "User, Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] RoomData data)
    {
        // var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        // if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        
        var result = await Mediator.Send(new CreateRoomCommand(data));
        
        if (!result.IsSuccess)
        {
            Console.WriteLine("A GRESIT BAAAAAA");
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }
        return CreatedAtAction(nameof(Create), result);
    }
    
}