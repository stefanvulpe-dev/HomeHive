namespace HomeHive.UI.Utils.Responses;

public class BaseApiResponse(string? message, bool isSuccess)
{
    protected BaseApiResponse() : this(null, true)
    {
    }

    public bool IsSuccess { get; init; } = isSuccess;
    public string? Message { get; set; } = message;

    public Dictionary<string, string>? ValidationErrors { get; init; }
}