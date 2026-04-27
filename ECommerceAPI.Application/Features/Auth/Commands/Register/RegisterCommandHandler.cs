using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Auth.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(IIdentityService identityService) : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    public Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken) =>
        identityService.RegisterAsync(request.FullName, request.Email, request.Password, cancellationToken);
}
