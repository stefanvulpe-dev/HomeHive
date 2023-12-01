namespace HomeHive.Domain.Common;

public sealed class Result<T> where T : class
{
    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; private set; }
    public T Value { get; private set; }
    public string Error { get; private set; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null!);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, null!, error);
    }
}

public sealed class Result
{
    private Result(bool isSuccess, string message, List<string> errors)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors;
    }

    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public List<string> Errors { get; private set; }
    
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

    public static Result Failure(string message, List<string> errors)
    {
        return new Result(false, message, errors);
    }
}