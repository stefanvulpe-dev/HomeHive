using HomeHive.Application.Features.Commands.CreateUser;
using HomeHive.Application.Features.Commands.DeleteUser;
using HomeHive.Application.Features.Commands.UpdateUser;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class UsersController: ApiBaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return CreatedAtAction(nameof(Create), result);
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(DeleteUserCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return NoContent();
    }
    
    [HttpPut("{userId}", Name = "Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute]Guid userId, [FromBody] UpdateUserCommand command)
    {
        command.UserId = userId;
        var result = await Mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}