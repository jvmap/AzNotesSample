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
        private readonly IBlobContainerClientFactory _factory;

        public BlobStorage(IBlobContainerClientFactory factory)
        {
            _factory = factory;
        }

        public string DescriptiveText => $"Azure Blob Storage (with {_factory.DescriptiveText})";

        public async Task<string> LoadAsync()
        {
            BlobClient bc = await CreateBlobClientAsync();
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
            BlobClient bc = await CreateBlobClientAsync();
            await bc.UploadAsync(new BinaryData(text), overwrite: true);
        }

        private async Task<BlobClient> CreateBlobClientAsync()
        {
            BlobContainerClient bcc = _factory.CreateBlobContainerClient(BLOB_CONTAINER_NAME);
            await bcc.CreateIfNotExistsAsync();
            return bcc.GetBlobClient(BLOB_NAME);
        }
    }
}
