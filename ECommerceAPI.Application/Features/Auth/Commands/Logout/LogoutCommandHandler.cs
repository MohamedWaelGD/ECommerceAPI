using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Results;
using MediatR;

namespace ECommerceAPI.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler(IIdentityService identityService) : IRequestHandler<LogoutCommand, Result>
{
    public Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken) =>
        identityService.LogoutAsync(request.UserId, request.RefreshToken, cancellationToken);
}
