namespace HomeHive.Domain.Common;

public sealed class Result<T> where T : class
{
    private Result(bool isSuccess, string message, Dictionary<string, string>? validationErrors)
    {
        IsSuccess = isSuccess;
        Message = message;
        ValidationErrors = validationErrors;
    }

    private Result(bool isSuccess, T value, string message)
    {
        IsSuccess = isSuccess;
        Value = value;
        Message = message;
    }

    public bool IsSuccess { get; private set; }
    public T Value { get; private set; } = null!;
    public string Message { get; private set; }

    public Dictionary<string, string>? ValidationErrors { get; private set; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null!);
    }

    public static Result<T> Success(T value, string message)
    {
        return new Result<T>(true, value, message);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, null!, error);
    }

    public static Result<T> Failure(string message, Dictionary<string, string> validationErrors)
    {
        return new Result<T>(false, message, validationErrors);
    }
}

public class Result
{
    private Result(bool isSuccess, string message, Dictionary<string, string>? validationErrors)
    {
        IsSuccess = isSuccess;
        Message = message;
        ValidationErrors = validationErrors;
    }

    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public Dictionary<string, string>? ValidationErrors { get; private set; }

    public static Result Success()
    {
        return new Result(true, null!, null!);
    }

    public static Result Success(string message)
    {
        return new Result(true, message, null!);
    }

    public static Result Failure(string message)
    {
        return new Result(false, message, null!);
    }

    public static Result Failure(string message, Dictionary<string, string> errors)
    {
        return new Result(false, message, errors);
    }
}