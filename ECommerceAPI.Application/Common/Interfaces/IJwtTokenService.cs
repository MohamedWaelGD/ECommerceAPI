using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Common.Interfaces;

public interface IJwtTokenService
{
    Task<(string AccessToken, DateTime ExpiresAt)> CreateAccessTokenAsync(User user);
    string CreateRefreshToken();
    DateTime GetRefreshTokenExpiration();
}
