namespace HomeHive.Domain.Common;

public sealed class Result<T> where T : class
{
    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public bool IsSuccess { get; private set;}
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