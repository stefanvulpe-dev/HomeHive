using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Features.Utilities.Commands.CreateUtility;
using HomeHive.Application.Features.Utilities.Commands.DeleteUtilityById;
using HomeHive.Application.Features.Utilities.Commands.UpdateUtility;
using HomeHive.Application.Features.Utilities.Queries.GetAllUtilities;
using HomeHive.Domain.Common.EntitiesUtils.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class UtilitiesController(
    ITokenCacheService tokenCacheService,
    ILogger<EstatesController> logger): ApiBaseController
{
    [Authorize(Roles = "User, Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] UtilityData data)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });
        Console.WriteLine(data.UtilityName);

        var result = await Mediator.Send(new CreateUtilityCommand(data));
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(Create), result);
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpPut ("{utilityId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid utilityId, [FromBody] UtilityData data)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new UpdateUtilityCommand(utilityId, data));
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpDelete("{utilityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid utilityId)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new DeleteUtilityByIdCommand(utilityId));
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new GetAllUtilitiesQuery());
        if (!result.IsSuccess)
        {
            foreach (var (field, error) in result.ValidationsErrors!)
                logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }

        return Ok(result);
    }
}