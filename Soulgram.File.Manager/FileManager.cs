using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace Soulgram.File.Manager;

public class FileManager : IFileManager
{
    private readonly BlobStorageOptions _storageOptions;
    private readonly IContainerNameResolver _containerNameResolver; 
    
    public FileManager(
        IOptions<BlobStorageOptions> storageOptions,
        IContainerNameResolver containerNameResolver)
    {
        _containerNameResolver = containerNameResolver;
        _storageOptions = storageOptions.Value;
    }

    public async Task<string> UploadFileAsync(FileInfo file, string userId)
    {
        try
        {
            var containerName = _containerNameResolver.GetContainerName(file.ContentType, userId);
            var containerClient = await GetContainerClient(file, containerName);

            var response = await containerClient.UploadAsync(file.Content,
                new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                });

            return containerClient.Uri.ToString();
        }
        finally
        {
            await file.Content.DisposeAsync();
        }
    }

    private async Task<BlobClient> GetContainerClient(FileInfo file, string containerName)
    {
        var blobContainerClient = new BlobContainerClient(_storageOptions.ConnectionString, containerName);

        await blobContainerClient.CreateIfNotExistsAsync();

        var containerClient = blobContainerClient.GetBlobClient(file.Name);
        return containerClient;
    }
}