using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Results;
using MediatR;

namespace ECommerceAPI.Application.Features.Auth.Commands.Logout;

public sealed record LogoutCommand(Guid UserId, string RefreshToken) : IRequest<Result>;
