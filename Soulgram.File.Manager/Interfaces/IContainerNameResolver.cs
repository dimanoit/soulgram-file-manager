namespace Soulgram.File.Manager;

public interface IContainerNameResolver
{
    public string GetContainerName(string contentType, string userId);
}