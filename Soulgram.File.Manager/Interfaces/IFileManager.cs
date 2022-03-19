using Azure.Storage.Blobs.Models;

namespace Soulgram.File.Manager;

public interface IFileManager
{
     Task<string> UploadFileAsync(FileInfo file, string userId);
     Task<IEnumerable<string>> UploadFilesAndGetIds(IEnumerable<FileInfo> files, string userId); 
     Task<string> UploadFileAsync(FileInfo file, string userId, BlobUploadOptions options);
}