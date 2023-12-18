using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Identity;
using HomeHive.Application.Models;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class AuthenticationController(
    IAuthService authService,
    ITokenCacheService tokenCacheService,
    ILogger<AuthenticationController> logger)
    : ApiBaseController
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Invalid payload");
            return BadRequest(new
            {
                error = "Invalid payload"
            });
        }

        var loginResult = await authService.Login(model);

        if (!loginResult.IsSuccess)
        {
            logger.LogError(loginResult.Message);
            return BadRequest(loginResult);
        }

        return Ok(loginResult);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationModel model)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Invalid payload");
            return BadRequest(ModelState);
        }

        var registrationResult = await authService.Register(model, UserRole.User);

        if (!registrationResult.IsSuccess) return BadRequest(registrationResult);

        return CreatedAtAction(nameof(Register), registrationResult);
    }

    [Authorize(Roles = "User, Admin")]
    [HttpDelete]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        var isTokenRevoked = await tokenCacheService.IsTokenRevokedAsync();
        if (isTokenRevoked) return Unauthorized(new { message = "Token is revoked" });

        var result = await authService.Logout();
        if (!result.IsSuccess)
        {
            logger.LogError(result.Message);
            return BadRequest(result);
        }

        return NoContent();
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var result = await authService.Refresh();
        if (!result.IsSuccess)
        {
            logger.LogError(result.Message);
            return Unauthorized(result);
        }

        return Ok(result);
    }
}