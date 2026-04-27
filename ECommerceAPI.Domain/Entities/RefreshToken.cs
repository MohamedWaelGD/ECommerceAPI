namespace ECommerceAPI.Domain.Entities;

public class RefreshToken
{
    private RefreshToken() { }

    private RefreshToken(Guid id, Guid userId, string token, DateTime expiresAt)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public bool IsExpired => ExpiresAt <= DateTime.UtcNow;
    public bool IsActive => !IsRevoked && !IsExpired;

    public static RefreshToken Create(Guid userId, string token, DateTime expiresAt) => new(Guid.NewGuid(), userId, token, expiresAt);

    public void Revoke() => IsRevoked = true;
}
