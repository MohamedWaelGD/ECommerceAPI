namespace ECommerceAPI.Application.Features.Auth.Dtos;

public sealed record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto User);
