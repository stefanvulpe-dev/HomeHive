using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Features.Users.Commands.DeleteUserById;
using HomeHive.Application.Features.Users.Queries.GetAllUsers;
using HomeHive.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class UsersController
(ICurrentUserService currentUserService, ITokenCacheService tokenCacheService,
    ILogger<UsersController> logger) : ApiBaseController
{
    [Authorize(Roles = "User, Admin")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete()
    {
        var result = await Mediator.Send(new DeleteUserByIdCommand(currentUserService.GetCurrentUserId()));
        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return BadRequest(new { errors = result.ValidationsErrors });
        }

        return NoContent();
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new GetUserByIdQuery(currentUserService.GetCurrentUserId()));

        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return BadRequest(new { errors = result.ValidationsErrors });
        }

        return Ok(new { user = result.User });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllUsersQuery());

        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return NotFound(new { errors = result.ValidationsErrors });
        }

        return Ok(new { users = result.Users });
    }
}