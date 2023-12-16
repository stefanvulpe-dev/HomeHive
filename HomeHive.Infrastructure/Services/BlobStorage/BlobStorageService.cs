using Azure.Storage.Blobs;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Domain.Common;
using Microsoft.Extensions.Options;

namespace HomeHive.Infrastructure.Services.BlobStorage;

public class BlobStorageService(BlobServiceClient blobServiceClient, IOptions<BlobStorageOptions> options)
    : IBlobStorageService
{
    public async Task<Result<Stream>> GetBlobAsync(string blobName, CancellationToken cancellationToken = default)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(options.Value.ContainerName);

        var blobClient = blobContainerClient.GetBlobClient(blobName);

        var response = await blobClient.DownloadAsync(cancellationToken);

        return !response.HasValue
            ? Result<Stream>.Failure("Blob not found")
            : Result<Stream>.Success(response.Value.Content);
    }

    public async Task<Result<IEnumerable<Stream>>> GetBlobsAsync(CancellationToken cancellationToken = default)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(options.Value.ContainerName);

        var blobs = new List<Stream>();

        await foreach (var blobItem in blobContainerClient.GetBlobsAsync(cancellationToken: cancellationToken))
        {
            var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);

            var response = await blobClient.DownloadAsync(cancellationToken);

            if (response.HasValue) blobs.Add(response.Value.Content);
        }

        return blobs.Count == 0
            ? Result<IEnumerable<Stream>>.Failure("No blobs found")
            : Result<IEnumerable<Stream>>.Success(blobs);
    }

    public async Task<bool> UploadBlobAsync(string blobName, Stream content,
        CancellationToken cancellationToken = default)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(options.Value.ContainerName);

        var blobClient = blobContainerClient.GetBlobClient(blobName);

        var response = await blobClient.UploadAsync(content, cancellationToken);

        return response.HasValue;
    }

    public async Task DeleteBlobAsync(string blobName, CancellationToken cancellationToken = default)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(options.Value.ContainerName);

        var blobClient = blobContainerClient.GetBlobClient(blobName);

        await blobClient.DeleteAsync(cancellationToken: cancellationToken);
    }
}