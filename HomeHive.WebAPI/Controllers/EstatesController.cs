using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Features.Estates.Commands.DeleteEstateById;
using HomeHive.Application.Features.Estates.Commands.UpdateEstate;
using HomeHive.Application.Features.Estates.Queries.GetAllEstates;
using HomeHive.Application.Features.Estates.Queries.GetEstateById;
using HomeHive.Application.Features.Users.Commands.CreateEstate;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class EstatesController(ICurrentUserService currentUserService) : ApiBaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] EstateData data)
    {
        var result = await Mediator.Send(new CreateEstateCommand(data with {OwnerId = currentUserService.GetCurrentUserId()}));
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(Create), result);
    }
    
    [HttpPut("{estateId}", Name = "Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid estateId, [FromBody] EstateData data)
    {
        var result = await Mediator.Send(new UpdateEstateCommand(estateId, data));
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid estateId)
    {
        var result = await Mediator.Send(new DeleteEstateByIdCommand(estateId));
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return NoContent();
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet("{estateId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid estateId)
    {
        var result = await Mediator.Send(new GetEstateByIdQuery(estateId));

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllEstatesQuery());

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}