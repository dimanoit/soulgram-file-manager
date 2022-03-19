namespace Soulgram.File.Manager;

public record BlobStorageOptions
{
    public string ConnectionString { get; init; }
    public string? ImagesContainer { get; init; }
    public string? VideoContainer { get; init; }
    public string? TextContainer { get; init; }
    public string UnknownContainer { get; init; }
}