using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeHive.WebAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ApiBaseController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator
        ??= HttpContext.RequestServices
            .GetRequiredService<ISender>();
}