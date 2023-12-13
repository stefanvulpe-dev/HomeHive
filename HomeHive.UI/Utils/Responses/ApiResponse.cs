namespace HomeHive.UI.Utils.Responses;

public sealed class ApiResponse<T> : BaseApiResponse
{
    public T? Value { get; set; }
}