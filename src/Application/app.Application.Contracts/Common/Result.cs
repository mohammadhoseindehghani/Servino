namespace app.Application.Contracts.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string? Message { get; }
    public string? ErrorCode { get; }

    protected Result(bool isSuccess, string? message, string? errorCode)
    {
        IsSuccess = isSuccess;
        Message = message;
        ErrorCode = errorCode;
    }

    public static Result Success(string? message = null) =>
        new(true, message, null);

    public static Result Failure(string message, string? errorCode = null) =>
        new(false, message, errorCode);
}

public class Result<T> : Result
{
    public T? Data { get; }

    private Result(bool isSuccess, string? message, string? errorCode, T? data)
        : base(isSuccess, message, errorCode)
    {
        Data = data;
    }

    public static Result<T> Success(T? data = default, string? message = null) =>
        new(true, message, null, data);

    public static Result<T> Failure(string message, string? errorCode = null, T? data = default) =>
        new(false, message, errorCode, data);

    public static implicit operator Result<T>(T data) =>
        Success(data);
}