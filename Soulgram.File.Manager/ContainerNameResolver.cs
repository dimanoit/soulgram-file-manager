using Microsoft.Extensions.Options;

namespace Soulgram.File.Manager;

public class ContainerNameResolver : IContainerNameResolver
{
    private readonly BlobStorageOptions _storageOptions;
    private readonly Dictionary<string, string?> _contentTypeToContainerNameDictionary;
        
    public ContainerNameResolver(IOptions<BlobStorageOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
        _contentTypeToContainerNameDictionary = new Dictionary<string, string?>()
        {
            {KnownContentTypes.Image, _storageOptions.ImagesContainer},
            {KnownContentTypes.Text, _storageOptions.TextContainer},
            {KnownContentTypes.Video, _storageOptions.VideoContainer},
        };
    }

    public string GetContainerName(string contentType, string userId)
    {
        // Trim like "text/csv" -> "text" 
        var markerWithoutFileExtension = contentType.Substring(0, 
            contentType.IndexOf("/", StringComparison.Ordinal));

        _contentTypeToContainerNameDictionary.TryGetValue(markerWithoutFileExtension, out var rawContainerName);

        return string.IsNullOrEmpty(rawContainerName)
            ? BuildContainerName(_storageOptions.UnknownContainer, userId)
            : BuildContainerName(rawContainerName, userId);
    }
    
    private string BuildContainerName(string? containerName, string userId) => $"{containerName}-{userId}";
}