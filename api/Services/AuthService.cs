using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.Models;
using api.Models.Auth;
using api.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services;

public class AuthService(IUserService userService, IOptions<AuthOptions> authOptions) : IAuthService
{
    private const int PasswordSaltSize = 16;
    private const int PasswordKeySize = 32;
    private const int PasswordIterations = 100_000;

    private readonly AuthOptions _authOptions = authOptions.Value;

    public async Task<ServiceResult<AuthSessionResult>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await userService.GetByEmailIncludingDeletedAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            return ServiceResult<AuthSessionResult>.Fail("email_in_use", "Email ja cadastrado.");
        }

        var passwordHash = HashPassword(request.Password);
        var user = await userService.CreateAsync(request.Name, request.Email, passwordHash, cancellationToken);

        return await BuildSessionResultAsync(user, rotateSession: true, cancellationToken);
    }

    public async Task<ServiceResult<AuthSessionResult>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userService.GetActiveByEmailAsync(request.Email, cancellationToken);
        if (user is null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            return ServiceResult<AuthSessionResult>.Fail("invalid_credentials", "Credenciais invalidas.");
        }

        return await BuildSessionResultAsync(user, rotateSession: true, cancellationToken);
    }

    public async Task<ServiceResult<AuthSessionResult>> RefreshAsync(string? sessionToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sessionToken))
        {
            return ServiceResult<AuthSessionResult>.Fail("invalid_session", "Sessao invalida.");
        }

        var user = await userService.GetBySessionTokenAsync(sessionToken, cancellationToken);
        if (user is null)
        {
            return ServiceResult<AuthSessionResult>.Fail("invalid_session", "Sessao invalida ou expirada.");
        }

        var result = CreateAuthResponse(user);

        return ServiceResult<AuthSessionResult>.Ok(new AuthSessionResult
        {
            Response = result,
            SessionToken = null,
            SessionExpiresAt = user.SessionExpiresAt
        });
    }

    public async Task<ServiceResult<bool>> LogoutAsync(string? sessionToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sessionToken))
        {
            return ServiceResult<bool>.Ok(true);
        }

        var user = await userService.GetBySessionTokenAsync(sessionToken, cancellationToken);
        if (user is null)
        {
            return ServiceResult<bool>.Ok(true);
        }

        await userService.ClearSessionAsync(user, cancellationToken);
        return ServiceResult<bool>.Ok(true);
    }

    private async Task<ServiceResult<AuthSessionResult>> BuildSessionResultAsync(User user, bool rotateSession, CancellationToken cancellationToken)
    {
        string? sessionToken = null;
        DateTime? sessionExpiresAt = user.SessionExpiresAt;

        if (rotateSession)
        {
            sessionToken = GenerateSessionToken();
            sessionExpiresAt = DateTime.UtcNow.AddHours(_authOptions.SessionHours);
            var sessionTokenHash = HashSessionToken(sessionToken);

            await userService.SetSessionAsync(user, sessionTokenHash, sessionExpiresAt.Value, cancellationToken);
        }

        var response = CreateAuthResponse(user);
        return ServiceResult<AuthSessionResult>.Ok(new AuthSessionResult
        {
            Response = response,
            SessionToken = sessionToken,
            SessionExpiresAt = sessionExpiresAt
        });
    }

    private AuthResponse CreateAuthResponse(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_authOptions.AccessTokenMinutes);
        var accessToken = GenerateAccessToken(user, expiresAt);

        return new AuthResponse
        {
            AccessToken = accessToken,
            AccessTokenExpiresAt = expiresAt,
            User = new UserSummary
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }
        };
    }

    private string GenerateAccessToken(User user, DateTime expiresAt)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.JwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _authOptions.Issuer,
            audience: _authOptions.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateSessionToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(48);
        return Convert.ToBase64String(bytes);
    }

    private static string HashSessionToken(string sessionToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(sessionToken));
        return Convert.ToHexString(bytes);
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(PasswordSaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA512, PasswordKeySize);

        return $"{PasswordIterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
        {
            return false;
        }

        byte[] salt;
        byte[] expected;

        try
        {
            salt = Convert.FromBase64String(parts[1]);
            expected = Convert.FromBase64String(parts[2]);
        }
        catch (FormatException)
        {
            return false;
        }

        var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA512, expected.Length);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
