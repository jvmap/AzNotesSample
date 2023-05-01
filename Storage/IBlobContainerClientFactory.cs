using Azure.Identity;
using Azure.Storage.Blobs;

namespace AzNotesSample.Storage
{
    public interface IBlobContainerClientFactory
    {
        BlobContainerClient CreateBlobContainerClient(string containerName);

        string DescriptiveText { get; }
    }
}
