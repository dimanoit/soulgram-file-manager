namespace Soulgram.File.Manager.Models;

public record FileInfo
{
    public string Name { get; init; }
    public string ContentType { get; init; }
    public Stream Content { get; init; }
}