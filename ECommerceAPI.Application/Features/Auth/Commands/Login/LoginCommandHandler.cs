using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Auth.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(IIdentityService identityService) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken) =>
        identityService.LoginAsync(request.Email, request.Password, cancellationToken);
}
