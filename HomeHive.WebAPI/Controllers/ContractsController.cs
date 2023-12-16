using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Features.Contracts.Commands.DeleteContractById;
using HomeHive.Application.Features.Contracts.Commands.UpdateContract;
using HomeHive.Application.Features.Contracts.Queries.GetAllContracts;
using HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;
using HomeHive.Application.Features.Contracts.Queries.GetContractById;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class ContractsController(
    ICurrentUserService currentUserService,
    ITokenCacheService tokenCacheService,
    ILogger<ContractsController> logger) : ApiBaseController
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

        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Error: {error}");
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
        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Error: {error}");
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
        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Error: {error}");
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

        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByUser()
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var userId = currentUserService.GetCurrentUserId();
        var result = await Mediator.Send(new GetAllContractsByUserIdQuery(userId));

        if (result.IsSuccess) return Ok(result);
        if (result.ValidationsErrors == null) return NotFound(result);
        foreach (var (field, error) in result.ValidationsErrors)
            logger.LogError($"Field: {field}, Message: {error}");
        return NotFound(result);

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

        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Error: {error}");
            return NotFound(result);
        }

        return Ok(result);
    }
    
}