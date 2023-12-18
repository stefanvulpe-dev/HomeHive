namespace HomeHive.Application.Responses;

public class BaseResponse
{
    protected BaseResponse()
    {
        IsSuccess = true;
    }

    public bool IsSuccess { get; init; }
    public string? Message { get; set; }
    public Dictionary<string, List<string>>? ValidationsErrors { get; init; }
}