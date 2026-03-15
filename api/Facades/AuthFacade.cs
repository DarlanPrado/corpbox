using api.Models.Auth;
using api.Services;

namespace api.Facades;

public class AuthFacade(IAuthService authService) : IAuthFacade
{
    public async Task<FacadeResult<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        if (!result.Success || result.Value is null)
        {
            return MapError(result.ErrorCode, result.ErrorMessage);
        }

        return FacadeResult<AuthResponse>.Created(
            result.Value.Response,
            result.Value.SessionToken,
            result.Value.SessionExpiresAt);
    }

    public async Task<FacadeResult<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        if (!result.Success || result.Value is null)
        {
            return MapError(result.ErrorCode, result.ErrorMessage);
        }

        return FacadeResult<AuthResponse>.Ok(
            result.Value.Response,
            result.Value.SessionToken,
            result.Value.SessionExpiresAt);
    }

    public async Task<FacadeResult<AuthResponse>> RefreshAsync(string? sessionToken, CancellationToken cancellationToken = default)
    {
        var result = await authService.RefreshAsync(sessionToken, cancellationToken);
        if (!result.Success || result.Value is null)
        {
            return MapError(result.ErrorCode, result.ErrorMessage);
        }

        return FacadeResult<AuthResponse>.Ok(result.Value.Response);
    }

    public async Task<FacadeResult<AuthResponse>> LogoutAsync(string? sessionToken, CancellationToken cancellationToken = default)
    {
        var result = await authService.LogoutAsync(sessionToken, cancellationToken);
        if (!result.Success)
        {
            return FacadeResult<AuthResponse>.Fail(500, "logout_failed", result.ErrorMessage ?? "Falha ao encerrar sessao.");
        }

        return FacadeResult<AuthResponse>.Cleared();
    }

    private static FacadeResult<AuthResponse> MapError(string? errorCode, string? errorMessage)
    {
        return errorCode switch
        {
            "email_in_use" => FacadeResult<AuthResponse>.Fail(409, errorCode, errorMessage ?? "Email ja cadastrado."),
            "invalid_credentials" => FacadeResult<AuthResponse>.Fail(401, errorCode, errorMessage ?? "Credenciais invalidas."),
            "invalid_session" => FacadeResult<AuthResponse>.Fail(401, errorCode, errorMessage ?? "Sessao invalida."),
            _ => FacadeResult<AuthResponse>.Fail(400, errorCode ?? "bad_request", errorMessage ?? "Requisicao invalida.")
        };
    }
}
