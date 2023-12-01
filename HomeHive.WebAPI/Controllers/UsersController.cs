using System.Security.Claims;
using HomeHive.Application.Features.Users.Commands.DeleteUserById;
using HomeHive.Application.Features.Users.Queries.GetAllUsers;
using HomeHive.Application.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class UsersController(ILogger<UsersController> logger) : ApiBaseController
{
    [Authorize(Roles = "User")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete()
    {
        var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
        if (userId == Guid.Empty)
        {
            logger.LogError("Invalid user id");
            return BadRequest(new
            {
                error = "Invalid user id"
            });
        }

        var result = await Mediator.Send(new DeleteUserByIdCommand(userId));
        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return BadRequest(new { errors = result.ValidationsErrors });
        }

        return NoContent();
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
        if (userId == Guid.Empty)
        {
            logger.LogError("Invalid user id");
            return BadRequest(new
            {
                error = "Invalid user id"
            });
        }

        var result = await Mediator.Send(new GetUserByIdQuery(userId));

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