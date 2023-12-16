using HomeHive.Domain.Common;

namespace HomeHive.Application.Contracts.Interfaces;

public interface IBlobStorageService
{
    Task<Result<Stream>> GetBlobAsync(string blobName, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Stream>>> GetBlobsAsync(CancellationToken cancellationToken = default);

    Task<bool> UploadBlobAsync(string blobName, Stream content,
        CancellationToken cancellationToken = default);

    Task DeleteBlobAsync(string blobName, CancellationToken cancellationToken = default);
}