namespace ECommerceAPI.Application.Common.Results;

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string? error, ResultErrorType errorType) : base(isSuccess, error, errorType)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, null, ResultErrorType.None);
    public static new Result<T> Failure(string error, ResultErrorType errorType = ResultErrorType.Validation) => new(false, default, error, errorType);
    public static new Result<T> NotFound(string error) => Failure(error, ResultErrorType.NotFound);
    public static new Result<T> Unauthorized(string error) => Failure(error, ResultErrorType.Unauthorized);
    public static new Result<T> Conflict(string error) => Failure(error, ResultErrorType.Conflict);
}
