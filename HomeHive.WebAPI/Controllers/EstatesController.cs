using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Features.Estates.Commands.CreateEstate;
using HomeHive.Application.Features.Estates.Commands.DeleteEstateById;
using HomeHive.Application.Features.Estates.Commands.UpdateEstate;
using HomeHive.Application.Features.Estates.Queries.GetAllEstates;
using HomeHive.Application.Features.Estates.Queries.GetEstateById;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class EstatesController(
    ICurrentUserService currentUserService,
    ITokenCacheService tokenCacheService,
    ILogger<EstatesController> logger)
    : ApiBaseController
{
    [Authorize(Roles = "User, Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] EstateData data)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var ownerId = currentUserService.GetCurrentUserId();
        var result = await Mediator.Send(new CreateEstateCommand(ownerId, data));
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Message: {error}");
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(Create), result);
    }

    [Authorize(Roles = "User, Admin")]
    [HttpPut("{estateId}", Name = "Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid estateId, [FromBody] EstateData data)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new UpdateEstateCommand(estateId, data));
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Message: {error}");
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize(Roles = "User, Admin")]
    [HttpDelete("{estateId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid estateId)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new DeleteEstateByIdCommand(estateId));
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Message: {error}");
            return BadRequest(result);
        }

        return NoContent();
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet("{estateId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid estateId)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new GetEstateByIdQuery(estateId));

        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Message: {error}");

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
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new GetAllEstatesQuery());

        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Message: {error}");

            return BadRequest(result);
        }

        return Ok(result);
    }
}