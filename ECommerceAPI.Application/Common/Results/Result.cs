namespace ECommerceAPI.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }
    public ResultErrorType ErrorType { get; }

    protected Result(bool isSuccess, string? error, ResultErrorType errorType)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
    }

    public static Result Success() => new(true, null, ResultErrorType.None);
    public static Result Failure(string error, ResultErrorType errorType = ResultErrorType.Validation) => new(false, error, errorType);
    public static Result NotFound(string error) => Failure(error, ResultErrorType.NotFound);
    public static Result Unauthorized(string error) => Failure(error, ResultErrorType.Unauthorized);
    public static Result Conflict(string error) => Failure(error, ResultErrorType.Conflict);
}

public enum ResultErrorType
{
    None,
    Validation,
    NotFound,
    Unauthorized,
    Conflict
}
