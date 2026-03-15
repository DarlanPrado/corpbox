namespace api.Facades;

public class FacadeResult<T>
{
    public bool Success { get; }

    public int StatusCode { get; }

    public string? ErrorCode { get; }

    public string? ErrorMessage { get; }

    public T? Value { get; }

    public string? SessionToken { get; }

    public DateTime? SessionExpiresAt { get; }

    public bool ClearSessionCookie { get; }

    private FacadeResult(
        bool success,
        int statusCode,
        T? value,
        string? errorCode,
        string? errorMessage,
        string? sessionToken,
        DateTime? sessionExpiresAt,
        bool clearSessionCookie)
    {
        Success = success;
        StatusCode = statusCode;
        Value = value;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        SessionToken = sessionToken;
        SessionExpiresAt = sessionExpiresAt;
        ClearSessionCookie = clearSessionCookie;
    }

    public static FacadeResult<T> Ok(T value, string? sessionToken = null, DateTime? sessionExpiresAt = null) =>
        new(true, 200, value, null, null, sessionToken, sessionExpiresAt, false);

    public static FacadeResult<T> Created(T value, string? sessionToken = null, DateTime? sessionExpiresAt = null) =>
        new(true, 201, value, null, null, sessionToken, sessionExpiresAt, false);

    public static FacadeResult<T> Fail(int statusCode, string errorCode, string errorMessage) =>
        new(false, statusCode, default, errorCode, errorMessage, null, null, false);

    public static FacadeResult<T> Cleared() =>
        new(true, 200, default, null, null, null, null, true);
}
