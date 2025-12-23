namespace Servino.Domain.Core._common;
public class Result<T>
{
    public bool IsSuccess { get; }
    public string? Message { get; }
    public string? ErrorCode { get; }
    public T? Data { get; }

    private Result(bool isSuccess, string? message, string? errorCode, T? data)
    {
        IsSuccess = isSuccess;
        Message = message;
        ErrorCode = errorCode;
        Data = data;
    }

    public static Result<T> Success(T? data = default, string? message = null) =>
        new(true, message, null, data);

    public static Result<T> Failure(string message, string? errorCode = null, T? data = default) =>
        new(false, message, errorCode, data);


    public static implicit operator Result<T>(T data) =>
        Success(data);
}