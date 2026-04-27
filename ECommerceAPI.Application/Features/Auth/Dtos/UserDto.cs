namespace ECommerceAPI.Application.Features.Auth.Dtos;

public sealed record UserDto(Guid Id, string FullName, string Email, IReadOnlyCollection<string> Roles);
