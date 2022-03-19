using Azure.Storage;
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

    public async Task<IEnumerable<string>> UploadFilesAndGetIds(IEnumerable<FileInfo> files, string userId)
    {
        var options = new BlobUploadOptions
        {
            TransferOptions = new StorageTransferOptions
            {
                MaximumConcurrency = 4,
                MaximumTransferSize = 50 * 1024 * 1024 // 50mb
            }
        };

        var uploadTasks = files
            .Select(f =>
            {
                options.HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = f.ContentType
                };
                
                return UploadFileAsync(f, userId, options);
            })
            .ToArray();

        await Task.WhenAll(uploadTasks);

        return uploadTasks.Select(t => t.Result);
    }

    public async Task<string> UploadFileAsync(FileInfo file, string userId)
    {
        return await UploadFileAsync(file, userId, new BlobUploadOptions()
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            }
        });
    }

    public async Task<string> UploadFileAsync(FileInfo file, string userId, BlobUploadOptions options)
    {
        try
        {
            var containerName = _containerNameResolver.GetContainerName(file.ContentType, userId);
            var containerClient = await GetContainerClient(file, containerName);

            await containerClient.UploadAsync(file.Content, options);

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

        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var containerClient = blobContainerClient.GetBlobClient(file.Name);
        return containerClient;
    }
}