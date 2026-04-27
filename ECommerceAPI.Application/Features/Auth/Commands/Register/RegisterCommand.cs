using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Auth.Dtos;
using MediatR;

namespace ECommerceAPI.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(string FullName, string Email, string Password) : IRequest<Result<AuthResponse>>;
