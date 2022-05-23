using Microsoft.Extensions.Options;
using Soulgram.File.Manager.Interfaces;
using Soulgram.File.Manager.Models;

namespace Soulgram.File.Manager;

public class ContainerNameResolver : IContainerNameResolver
{
    private readonly Dictionary<string, string?> _contentTypeToContainerNameDictionary;
    private readonly BlobStorageOptions _storageOptions;

    public ContainerNameResolver(IOptions<BlobStorageOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
        _contentTypeToContainerNameDictionary = new Dictionary<string, string?>
        {
            { KnownContentTypes.Image, _storageOptions.ImagesContainer },
            { KnownContentTypes.Text, _storageOptions.TextContainer },
            { KnownContentTypes.Video, _storageOptions.VideoContainer }
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

    private string BuildContainerName(string? containerName, string userId)
    {
        return $"{containerName}-{userId}";
    }
}