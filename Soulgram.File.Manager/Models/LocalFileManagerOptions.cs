namespace Soulgram.File.Manager.Models;

public record LocalFileManagerOptions
{
    public string FullPath { get; init; }
    public string RelatedPath { get; init; }
}
