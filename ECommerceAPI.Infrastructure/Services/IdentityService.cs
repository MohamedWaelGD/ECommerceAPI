using ECommerceAPI.Application.Common.Interfaces;
using ECommerceAPI.Application.Common.Results;
using ECommerceAPI.Application.Features.Auth.Dtos;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Infrastructure.Services;

public class IdentityService(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, IJwtTokenService jwtTokenService) : IIdentityService
{
    public async Task<Result<AuthResponse>> RegisterAsync(string fullName, string email, string password, CancellationToken cancellationToken)
    {
        var user = new User { Id = Guid.NewGuid(), FullName = fullName, UserName = email, Email = email };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded) return Result<AuthResponse>.Failure(string.Join(" ", result.Errors.Select(x => x.Description)));

        await userManager.AddToRoleAsync(user, "Customer");
        context.Carts.Add(Cart.Create(user.Id));
        await context.SaveChangesAsync(cancellationToken);

        return await CreateAuthResponseAsync(user, cancellationToken);
    }

    public async Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return Result<AuthResponse>.Failure("Invalid email or password.");

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded) return Result<AuthResponse>.Failure("Invalid email or password.");

        return await CreateAuthResponseAsync(user, cancellationToken);
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var token = await context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == refreshToken, cancellationToken);
        if (token is null || !token.IsActive) return Result<AuthResponse>.Failure("Invalid refresh token.");

        token.Revoke();
        return await CreateAuthResponseAsync(token.User, cancellationToken);
    }

    public async Task<Result> LogoutAsync(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        var token = await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.Token == refreshToken, cancellationToken);
        if (token is null) return Result.Failure("Refresh token not found.");

        token.Revoke();
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<UserDto>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return Result<UserDto>.Failure("User not found.");

        return Result<UserDto>.Success(await CreateUserDtoAsync(user));
    }

    private async Task<Result<AuthResponse>> CreateAuthResponseAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = await jwtTokenService.CreateAccessTokenAsync(user);
        var refreshToken = RefreshToken.Create(user.Id, jwtTokenService.CreateRefreshToken(), jwtTokenService.GetRefreshTokenExpiration());

        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(new AuthResponse(accessToken.AccessToken, refreshToken.Token, accessToken.ExpiresAt, await CreateUserDtoAsync(user)));
    }

    private async Task<UserDto> CreateUserDtoAsync(User user)
    {
        var roles = await userManager.GetRolesAsync(user);
        return new UserDto(user.Id, user.FullName, user.Email ?? string.Empty, roles.ToArray());
    }
}
