using HomeHive.Application.Contracts.Caching;
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

public class ContractsController
    (ICurrentUserService currentUserService, ITokenCacheService tokenCacheService, ILogger<ContractsController> logger) : ApiBaseController
{
    [Authorize(Roles = "User, Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(ContractData contractData)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        
        var userId = currentUserService.GetCurrentUserId();
        var result = await Mediator.Send(new CreateContractCommand(userId, contractData));
        
        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(Create), result);
    }

    [Authorize(Roles = "User, Admin")]
    [HttpPut("{contractId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid contractId, [FromBody] ContractData contractData)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        
        var result = await Mediator.Send(new UpdateContractCommand(contractId, contractData));
        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize(Roles = "User, Admin")]
    [HttpDelete("{contractId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid contractId)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        
        var result = await Mediator.Send(new DeleteContractByIdCommand(contractId));
        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return BadRequest(result);
        }

        return NoContent();
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet("{contractId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid contractId)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        
        var result = await Mediator.Send(new GetContractByIdQuery(contractId));

        if (!result.Success)
        {
            result.ValidationsErrors?.ForEach(error => logger.LogError(error));
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        
        var result = await Mediator.Send(new GetAllContractsQuery());

        if (!result.Success)
        {
            result.ValidationsErrors!.ForEach(error => logger.LogError(error));
            return NotFound(result);
        }

        return Ok(result);
    }
}