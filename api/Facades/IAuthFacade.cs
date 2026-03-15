using api.Models.Auth;

namespace api.Facades;

public interface IAuthFacade
{
    Task<FacadeResult<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<FacadeResult<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<FacadeResult<AuthResponse>> RefreshAsync(string? sessionToken, CancellationToken cancellationToken = default);

    Task<FacadeResult<AuthResponse>> LogoutAsync(string? sessionToken, CancellationToken cancellationToken = default);
}
