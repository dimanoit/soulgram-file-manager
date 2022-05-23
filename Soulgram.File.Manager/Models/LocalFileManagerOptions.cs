namespace Soulgram.File.Manager.Models;

public record LocalFileManagerOptions
{
    public string FullPath { get; init; } = null!;
    public string RelatedPath { get; init; } = null!;
}