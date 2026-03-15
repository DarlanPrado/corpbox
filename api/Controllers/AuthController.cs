using api.Facades;
using api.Models.Auth;
using api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthFacade authFacade, IOptions<AuthOptions> authOptions, IWebHostEnvironment environment) : ControllerBase
{
    private readonly AuthOptions _authOptions = authOptions.Value;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await authFacade.RegisterAsync(request, cancellationToken);
        return BuildResponse(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authFacade.LoginAsync(request, cancellationToken);
        return BuildResponse(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        Request.Cookies.TryGetValue(_authOptions.SessionCookieName, out var sessionToken);
        var result = await authFacade.RefreshAsync(sessionToken, cancellationToken);
        return BuildResponse(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        Request.Cookies.TryGetValue(_authOptions.SessionCookieName, out var sessionToken);
        var result = await authFacade.LogoutAsync(sessionToken, cancellationToken);
        return BuildResponse(result);
    }

    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult Me()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
            ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email)?.Value;

        return Ok(new
        {
            Id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
            Name = User.Identity?.Name,
            Email = email
        });
    }

    private IActionResult BuildResponse(FacadeResult<AuthResponse> result)
    {
        if (result.ClearSessionCookie)
        {
            Response.Cookies.Delete(_authOptions.SessionCookieName);
        }
        else if (!string.IsNullOrWhiteSpace(result.SessionToken) && result.SessionExpiresAt.HasValue)
        {
            Response.Cookies.Append(_authOptions.SessionCookieName, result.SessionToken, BuildCookieOptions(result.SessionExpiresAt.Value));
        }

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, new
            {
                error = result.ErrorCode,
                message = result.ErrorMessage
            });
        }

        if (result.StatusCode == 201 && result.Value is not null)
        {
            return StatusCode(201, result.Value);
        }

        return Ok(result.Value);
    }

    private CookieOptions BuildCookieOptions(DateTime expiresAt)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = !environment.IsDevelopment(),
            SameSite = SameSiteMode.Lax,
            Expires = expiresAt,
            IsEssential = true
        };
    }
}
