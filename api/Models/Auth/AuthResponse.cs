namespace api.Models.Auth;

public class AuthResponse
{
    public required string AccessToken { get; init; }

    public required DateTime AccessTokenExpiresAt { get; init; }

    public required UserSummary User { get; init; }
}

public class UserSummary
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Email { get; init; }
}
