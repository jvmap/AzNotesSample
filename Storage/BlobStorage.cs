using AzNotesSample.Configuration;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System;

namespace AzNotesSample.Storage
{
    public class BlobStorage : IStorage
    {
        private const string BLOB_CONTAINER_NAME = "aznotessample";
        private const string BLOB_NAME = "note";
        private readonly Task<BlobClient> _createBlobClientAsync;

        public BlobStorage(IBlobContainerClientFactory factory)
        {
            _createBlobClientAsync = CreateBlobClientAsync(factory);
            DescriptiveText = $"Azure Blob Storage (with {factory.DescriptiveText})";
        }

        public string DescriptiveText { get; private set; }

        public async Task<string> LoadAsync()
        {
            BlobClient bc = await _createBlobClientAsync;
            if (await bc.ExistsAsync())
            {
                var response = await bc.DownloadContentAsync();
                return response.Value.Content.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task SaveAsync(string text)
        {
            text ??= "";
            BlobClient bc = await _createBlobClientAsync;
            await bc.UploadAsync(new BinaryData(text), overwrite: true);
        }

        private static async Task<BlobClient> CreateBlobClientAsync(IBlobContainerClientFactory factory)
        {
            BlobContainerClient bcc = factory.CreateBlobContainerClient(BLOB_CONTAINER_NAME);
            await bcc.CreateIfNotExistsAsync();
            return bcc.GetBlobClient(BLOB_NAME);
        }
    }
}
