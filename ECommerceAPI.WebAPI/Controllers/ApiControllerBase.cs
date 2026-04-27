using ECommerceAPI.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.WebAPI.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult FromResult(Result result) => result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });

    protected ActionResult<T> FromResult<T>(Result<T> result)
    {
        if (result.IsSuccess && result.Value is not null) return Ok(result.Value);
        return BadRequest(new { error = result.Error });
    }

    protected Guid CurrentUserId => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
}
