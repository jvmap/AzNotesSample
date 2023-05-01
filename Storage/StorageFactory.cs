using AzNotesSample.Configuration;
using Microsoft.Extensions.Options;

namespace AzNotesSample.Storage
{
    internal class StorageFactory
    {
        internal static IStorage CreateStorage(IServiceProvider serviceProvider)
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<StorageOptions>>();
            string? storageAccountName = options.Value.STORAGE_ACCOUNT_NAME;
            if (string.IsNullOrEmpty(storageAccountName))
                return new MemoryStorage();
            else
                return new BlobStorage(
                    CreateBlobContainerClientFactory(
                        storageAccountName,
                        options.Value)
                    );
        }

        private static IBlobContainerClientFactory CreateBlobContainerClientFactory(
            string storageAccountName, 
            StorageOptions options)
        {
            if (options.MANAGED_IDENTITY == 1)
            {
                return new BlobContainerClientFactoryUsingDefaultAzureCredential(storageAccountName);
            }
            else
            {
                return new BlobContainerClientFactoryUsingAccessKey(
                    storageAccountName, 
                    options.STORAGE_ACCOUNT_KEY ?? "");
            }
        }
    }
}