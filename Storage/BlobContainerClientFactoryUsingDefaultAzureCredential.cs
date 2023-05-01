using Azure.Identity;
using Azure.Storage.Blobs;

namespace AzNotesSample.Storage
{
    public class BlobContainerClientFactoryUsingDefaultAzureCredential : IBlobContainerClientFactory
    {
        private readonly Uri _accountUri;

        public string DescriptiveText => "Managed Identity";

        public BlobContainerClientFactoryUsingDefaultAzureCredential(string accountName)
        {
            this._accountUri = GetAccountUri(accountName);
        }

        public BlobContainerClient CreateBlobContainerClient(string containerName)
        {
            return new BlobContainerClient(GetContainerUri(containerName), new DefaultAzureCredential());
        }

        private Uri GetContainerUri(string containerName)
        {
            return new Uri(_accountUri, containerName);
        }

        private static Uri GetAccountUri(string accountName)
        {
            return new Uri($"https://{accountName}.blob.core.windows.net");
        }
    }
}
