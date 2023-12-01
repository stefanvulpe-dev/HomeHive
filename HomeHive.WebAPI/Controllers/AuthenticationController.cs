using HomeHive.Application.Contracts.Identity;
using HomeHive.Application.Models;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

public class AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
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
            logger.LogError(loginResult.Error);
            return BadRequest(new { error = loginResult.Error });
        }
        
        return Ok(new { token = loginResult.Value });
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
}