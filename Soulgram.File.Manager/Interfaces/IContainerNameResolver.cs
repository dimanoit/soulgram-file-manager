namespace Soulgram.File.Manager.Interfaces;

public interface IContainerNameResolver
{
    public string GetContainerName(string contentType, string userId);
}