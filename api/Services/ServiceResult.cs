namespace api.Services;

public class ServiceResult<T>
{
    public bool Success { get; }

    public string? ErrorCode { get; }

    public string? ErrorMessage { get; }

    public T? Value { get; }

    private ServiceResult(bool success, T? value, string? errorCode, string? errorMessage)
    {
        Success = success;
        Value = value;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult<T> Ok(T value) => new(true, value, null, null);

    public static ServiceResult<T> Fail(string errorCode, string errorMessage) => new(false, default, errorCode, errorMessage);
}
