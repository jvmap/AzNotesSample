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
            if (string.IsNullOrEmpty(options.Value.AccountName))
                return new MemoryStorage();
            else
            {
                var scs = new StorageConnectionString(options);
                return new BlobStorage(scs);
            }
        }
    }
}