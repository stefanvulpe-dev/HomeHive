using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Identity;
using HomeHive.Application.Models;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class AuthenticationController(IAuthService authService, ITokenCacheService tokenCacheService,
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
            return BadRequest(new { error = loginResult.Message });
        }

        return Ok(new { accessToken = loginResult.AccessToken, refreshToken = loginResult.RefreshToken });
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationModel model)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Invalid payload");
            return BadRequest(new
            {
                error = "Invalid payload"
            });
        }

        var registrationResult = await authService.Register(model, UserRole.User);

        if (!registrationResult.IsSuccess)
        {
            registrationResult.Errors.ForEach(error => logger.LogError(error));
            return BadRequest(new { errors = registrationResult.Errors });
        }

        return CreatedAtAction(nameof(Register), model);
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
            return BadRequest(new { error = result.Message });
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
            return Unauthorized(new { error = result.Message });
        }

        return Ok(new { accessToken = result.AccessToken, refreshToken = result.RefreshToken });
    }
}