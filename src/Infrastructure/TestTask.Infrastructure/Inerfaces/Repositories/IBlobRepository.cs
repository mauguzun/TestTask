using Azure.Storage.Blobs.Models;
using Azure;

namespace TestTask.Infrastructure.Inerfaces.Repositories
{
    public interface IBlobRepository
    {
        Task<Response<BlobDownloadResult>> GetBlobById(string id, CancellationToken cancellationToken);
        Task SaveBlobDataAsync(string filename, HttpContent payload, CancellationToken cancellationToken);
    }
}
