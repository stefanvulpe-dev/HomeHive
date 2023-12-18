namespace HomeHive.UI.Utils.Responses;

public sealed class ApiResponse<T>
{
    public T? Value { get; set; }
    public bool IsSuccess { get; init; }
    public string? Message { get; set; } 
    public Dictionary<string, string>? ValidationErrors { get; init; }
}

public sealed class ApiResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; set; } 
    public Dictionary<string, string>? ValidationErrors { get; init; }
}