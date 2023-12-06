using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Features.Contracts.Commands.DeleteContractById;
using HomeHive.Application.Features.Contracts.Commands.UpdateContract;
using HomeHive.Application.Features.Contracts.Queries.GetAllContracts;
using HomeHive.Application.Features.Contracts.Queries.GetContractById;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class ContractController 
    (ICurrentUserService currentUserService, ILogger<ContractController> logger) : ApiBaseController
{
    
    [Authorize(Roles = "User, Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateContractCommand command)
    {
        CreateContractCommand newCommand = new(command.Data with { UserId = currentUserService.GetCurrentUserId() });
        var result = await Mediator.Send(newCommand);
        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return BadRequest(new { errors = result.ValidationsErrors });
        }

        return Ok(result);
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpPut("{contractId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute]Guid contractId, [FromBody]ContractData contractData)
    {
        var result = await Mediator.Send(new UpdateContractCommand(contractId, contractData));
        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return BadRequest(new { errors = result.ValidationsErrors });
        }

        return Ok(result);
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteContractByIdCommand(id));
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
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await Mediator.Send(new GetContractByIdQuery(id));

        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return BadRequest(new { errors = result.ValidationsErrors });
        }

        return Ok(new { contract = result.Contract });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllContractsQuery());

        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return NotFound(new { errors = result.ValidationsErrors });
        }

        return Ok(new { contracts = result.Contracts });
    }

}