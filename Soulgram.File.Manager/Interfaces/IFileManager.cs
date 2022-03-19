namespace Soulgram.File.Manager;

public interface IFileManager
{
    public Task<string> UploadFileAsync(FileInfo file, string userId);
    public Task<IEnumerable<string>> UploadFilesAndGetIds(IEnumerable<FileInfo> files, string userId);
}