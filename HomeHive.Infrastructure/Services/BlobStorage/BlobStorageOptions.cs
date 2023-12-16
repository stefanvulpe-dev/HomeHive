namespace HomeHive.Infrastructure.Services.BlobStorage;

public class BlobStorageOptions
{
    public string? ConnectionString { get; init; }
    public string? ContainerName { get; init; }
    public string? AccountName { get; init; }
    public string? AccountKey { get; init; }
}