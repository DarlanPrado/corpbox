namespace api.Options;

public class AuthOptions
{
    public const string SectionName = "Auth";

    public string JwtSecret { get; set; } = string.Empty;

    public string Issuer { get; set; } = "CorpBox.Api";

    public string Audience { get; set; } = "CorpBox.Web";

    public int AccessTokenMinutes { get; set; } = 5;

    public int SessionHours { get; set; } = 12;

    public string SessionCookieName { get; set; } = "cbx_session";
}
