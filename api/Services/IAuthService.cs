using api.Models.Auth;

namespace api.Services;

public interface IAuthService
{
    Task<ServiceResult<AuthSessionResult>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<ServiceResult<AuthSessionResult>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<ServiceResult<AuthSessionResult>> RefreshAsync(string? sessionToken, CancellationToken cancellationToken = default);

    Task<ServiceResult<bool>> LogoutAsync(string? sessionToken, CancellationToken cancellationToken = default);
}

public class AuthSessionResult
{
    public required AuthResponse Response { get; init; }

    public string? SessionToken { get; init; }

    public DateTime? SessionExpiresAt { get; init; }
}
