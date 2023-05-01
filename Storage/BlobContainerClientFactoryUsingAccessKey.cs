using Azure.Identity;
using Azure.Storage.Blobs;

namespace AzNotesSample.Storage
{
    public class BlobContainerClientFactoryUsingAccessKey : IBlobContainerClientFactory
    {
        private readonly string _connectionString;

        public string DescriptiveText => "Access Key";

        public BlobContainerClientFactoryUsingAccessKey(string accountName, string accountKey)
        {
            _connectionString = GetConnectionString(accountName, accountKey);
        }

        public BlobContainerClient CreateBlobContainerClient(string containerName)
        {
            return new BlobContainerClient(_connectionString, containerName);
        }

        private static string GetConnectionString(string accountName, string accountKey)
        {
            return $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accountKey};";
        }
    }
}
