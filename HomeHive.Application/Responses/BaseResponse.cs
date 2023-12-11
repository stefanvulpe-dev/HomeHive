namespace HomeHive.Application.Responses;

public class BaseResponse
{
    protected BaseResponse()
    {
        IsSuccess = true;
    }

    public BaseResponse(string? message, bool isSuccess)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public bool IsSuccess { get; init; }
    public string? Message { get; set; }
    public Dictionary<string, string>? ValidationsErrors { get; init; }
}