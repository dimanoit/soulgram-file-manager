using Azure.Storage.Blobs.Models;
using FileInfo = Soulgram.File.Manager.Models.FileInfo;

namespace Soulgram.File.Manager.Interfaces;

public interface IFileManager
{
    Task DeleteFileAsync(string fileUrl);
    Task<string> UploadFileAsync(FileInfo file, string userId);
    Task<IEnumerable<string>> UploadFilesAndGetIds(IEnumerable<FileInfo> files, string userId);
    Task<string> UploadFileAsync(FileInfo file, string userId, BlobUploadOptions options);
}