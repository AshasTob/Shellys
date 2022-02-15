using Azure.Storage.Blobs;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IO;

namespace OrderTimeOutService.BlobService
{
    public class BlobStorage : IBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _blobContainerName;

        public BlobStorage(IConfigurationBuilder configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration.Build().GetConnectionString("BlobStorage"));
            _blobContainerName = configuration.Build().GetSection("Containers").GetSection("OrderTimeOutReport").Value;
        }

        public async Task UploadContentBlobAsync(string content, string fileName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            MemoryStream memoryStream = new MemoryStream(bytes);
            await blobClient.UploadAsync(memoryStream);
        }
    }
}
