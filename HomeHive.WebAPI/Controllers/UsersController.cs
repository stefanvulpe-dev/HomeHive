﻿using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Features.Users.Commands.DeleteUserById;
using HomeHive.Application.Features.Users.Queries.GetAllUsers;
using HomeHive.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class UsersController(
    ICurrentUserService currentUserService,
    ITokenCacheService tokenCacheService,
    ILogger<UsersController> logger) : ApiBaseController
{
    [Authorize(Roles = "User, Admin")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete()
    {
        var result = await Mediator.Send(new DeleteUserByIdCommand(currentUserService.GetCurrentUserId()));
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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new GetUserByIdQuery(currentUserService.GetCurrentUserId()));

        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await Mediator.Send(new GetAllUsersQuery());

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