using AzNotesSample.Configuration;
using Microsoft.Extensions.Options;

namespace AzNotesSample.Storage
{
    public class StorageConnectionString
    {
        private readonly string _value;

        public StorageConnectionString(IOptions<StorageOptions> options)
        {
            _value = GetStorageConnectionString(options.Value);
        }

        public string Value => _value;

        private static string GetStorageConnectionString(StorageOptions options)
        {
            return $"DefaultEndpointsProtocol=https;AccountName={options.STORAGE_ACCOUNT_NAME};AccountKey={options.STORAGE_ACCOUNT_KEY};";
        }
    }
}
