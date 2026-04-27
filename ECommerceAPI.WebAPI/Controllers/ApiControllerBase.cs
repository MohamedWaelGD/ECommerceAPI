using ECommerceAPI.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.WebAPI.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult FromResult(Result result) => result.IsSuccess ? NoContent() : ToErrorResult(result);

    protected ActionResult<T> FromResult<T>(Result<T> result)
    {
        if (result.IsSuccess && result.Value is not null) return Ok(result.Value);
        return ToErrorResult(result);
    }

    private ActionResult ToErrorResult(Result result) => result.ErrorType switch
    {
        ResultErrorType.NotFound => NotFound(new ApiErrorResponse(result.Error)),
        ResultErrorType.Unauthorized => Unauthorized(new ApiErrorResponse(result.Error)),
        ResultErrorType.Conflict => Conflict(new ApiErrorResponse(result.Error)),
        _ => BadRequest(new ApiErrorResponse(result.Error))
    };

    protected Guid CurrentUserId => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
}

public sealed record ApiErrorResponse(string? Error);
