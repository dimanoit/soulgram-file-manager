using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Soulgram.File.Manager.Interfaces;
using Soulgram.File.Manager.Models;
using FileInfo = Soulgram.File.Manager.Models.FileInfo;

namespace Soulgram.File.Manager;

public class LocalFileManager : IFileManager
{
    private readonly LocalFileManagerOptions _fileOptions;

    public LocalFileManager(IOptions<LocalFileManagerOptions> fileOptions)
    {
        _fileOptions = fileOptions.Value;
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        await Task.Run(() =>
        {
            var fullName = _fileOptions.FullPath + GetFileName(fileUrl);
            if (System.IO.File.Exists(fullName))
            {
                System.IO.File.Delete(fullName);
            }
        });
    }

    public async Task<string> UploadFileAsync(FileInfo file, string userId)
    {
        var fileType = file.Name.Split('.').Last();
        var fileName = Guid.NewGuid() + "." + fileType;
        var fullFilePath = _fileOptions.FullPath + fileName;

        await using (var fileStream = System.IO.File.Create(fullFilePath))
        {
            file.Content.Seek(0, SeekOrigin.Begin);
            await file.Content.CopyToAsync(fileStream);
            await file.Content.DisposeAsync();
        }

        return _fileOptions.RelatedPath + fileName;
    }

    public async Task<IEnumerable<string>> UploadFilesAndGetIds(IEnumerable<FileInfo> files, string userId)
    {
        var uploadTasks = files
            .Select(f => UploadFileAsync(f, userId))
            .ToArray();

        await Task.WhenAll(uploadTasks);

        return uploadTasks.Select(t => t.Result);
    }

    public Task<string> UploadFileAsync(FileInfo file, string userId, BlobUploadOptions options)
    {
        //TODO should cut this method from interface 
        throw new NotImplementedException();
    }

    private string GetFileName(string url)
    {
        var indexOfStartingFileName = url.LastIndexOf("/", StringComparison.Ordinal) + 1;
        var fileName = url.Substring(indexOfStartingFileName);
        return fileName;
    }
}