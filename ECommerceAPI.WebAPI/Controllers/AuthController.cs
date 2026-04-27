using ECommerceAPI.Application.Features.Auth.Commands.Login;
using ECommerceAPI.Application.Features.Auth.Commands.Logout;
using ECommerceAPI.Application.Features.Auth.Commands.RefreshToken;
using ECommerceAPI.Application.Features.Auth.Commands.Register;
using ECommerceAPI.Application.Features.Auth.Dtos;
using ECommerceAPI.Application.Features.Auth.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.WebAPI.Controllers;

[Route("api/auth")]
public class AuthController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Register(RegisterCommand command, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(command, cancellationToken));

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login(LoginCommand command, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(command, cancellationToken));

    [HttpPost("refresh-token")]
    [Authorize]
    public async Task<ActionResult<AuthResponse>> RefreshToken(RefreshTokenCommand command, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(command, cancellationToken));

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout(RefreshTokenCommand request, CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new LogoutCommand(CurrentUserId, request.RefreshToken), cancellationToken));

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Me(CancellationToken cancellationToken) =>
        FromResult(await mediator.Send(new GetCurrentUserQuery(CurrentUserId), cancellationToken));
}
