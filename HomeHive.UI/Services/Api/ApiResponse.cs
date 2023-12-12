namespace HomeHive.UI.Services.Api;

public sealed class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Value { get; set; }
    public Dictionary<string, string>? ValidationErrors { get; set; }
}

public sealed class ApiResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, string>? ValidationErrors { get; set; }
}