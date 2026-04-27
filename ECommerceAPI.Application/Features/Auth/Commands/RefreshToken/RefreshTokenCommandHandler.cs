using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Auth.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(IIdentityService identityService) : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) =>
        identityService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
}
