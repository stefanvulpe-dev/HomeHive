using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Features.Users.Commands.DeleteProfilePicture;
using HomeHive.Application.Features.Users.Commands.DeleteUserById;
using HomeHive.Application.Features.Users.Commands.UploadProfilePicture;
using HomeHive.Application.Features.Users.Queries.GetAllUsers;
using HomeHive.Application.Features.Users.Queries.GetProfilePicture;
using HomeHive.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class UsersController(
    ICurrentUserService currentUserService,
    ITokenCacheService tokenCacheService,
    ILogger<UsersController> logger) : ApiBaseController
{
    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete()
    {
        var result = await Mediator.Send(new DeleteUserByIdCommand(currentUserService.GetCurrentUserId()));
        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Message: {error}");
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
                    logger.LogError($"Field: {field}, Message: {error}");
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
                    logger.LogError($"Field: {field}, Message: {error}");
            return NotFound(result);
        }

        return Ok(result);
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPost("profile-picture")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var userId = currentUserService.GetCurrentUserId();

        var result = await Mediator.Send(new UploadProfilePictureCommand(userId,
            file.OpenReadStream(), file.ContentType));

        if (!result.IsSuccess)
        {
            if (result.ValidationsErrors != null)
                foreach (var (field, error) in result.ValidationsErrors)
                    logger.LogError($"Field: {field}, Error: {error}");
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(UploadProfilePicture), result);
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("profile-picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProfilePicture()
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var userId = currentUserService.GetCurrentUserId();

        var result = await Mediator.Send(new GetProfilePictureQuery(userId));

        if (!result.IsSuccess) return BadRequest(result);

        return File(result.Content!, "image/jpeg");
    }

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("profile-picture")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var userId = currentUserService.GetCurrentUserId();

        var result = await Mediator.Send(new DeleteProfilePictureCommand(userId));

        if (!result.IsSuccess) return BadRequest(result);

        return NoContent();
    }
}