using HomeHive.Application.Features.Commands.CreateUser;
using HomeHive.Application.Features.Commands.UpdateUser;
using HomeHive.Application.Features.Commands.DeleteUserByEmail;
using HomeHive.Application.Features.Commands.DeleteUserById;
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
    public async Task<IActionResult> Delete(DeleteUserByEmailCommand command)
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
    
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid userId)
    {
        var result = await Mediator.Send(new DeleteUserByIdCommand(userId));
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return NoContent();
    }
}