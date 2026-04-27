using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Auth.Dtos;

namespace ECommerceAPI.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<AuthResponse>> RegisterAsync(string fullName, string email, string password, CancellationToken cancellationToken);
    Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken cancellationToken);
    Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<Result> LogoutAsync(Guid userId, string refreshToken, CancellationToken cancellationToken);
    Task<Result<UserDto>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken);
}
