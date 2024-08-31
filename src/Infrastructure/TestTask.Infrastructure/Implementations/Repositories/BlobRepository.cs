using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using TestTask.Infrastructure.Inerfaces.Repositories;

namespace TestTask.Infrastructure.Implementations.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private readonly BlobContainerClient _containerClient;
        public BlobRepository(IConfiguration configuration)
        {
            var blobServiceClient = new BlobServiceClient(configuration["Values:AzureWebJobsStorage"]);
            _containerClient = blobServiceClient.GetBlobContainerClient(configuration["AzureStorageName"]);
            _containerClient.CreateIfNotExists();
        }

        public async Task<Response<BlobDownloadResult>> GetBlobById(string id, CancellationToken cancellationToken)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(id);

            if (await blobClient.ExistsAsync())
                return await blobClient.DownloadContentAsync();
                
            return null;
        }

        public async Task SaveBlobDataAsync(string fileName, HttpContent payload, CancellationToken cancellationToken)
        {
            using (var stream = new MemoryStream(await payload.ReadAsByteArrayAsync()))
            { 
               await _containerClient.UploadBlobAsync(fileName, stream ,cancellationToken);
            }
        }
    }

   
}
