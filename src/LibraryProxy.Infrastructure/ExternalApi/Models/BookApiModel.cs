namespace LibraryProxy.Infrastructure.ExternalApi.Models;

public sealed class BookApiModel
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int PageCount { get; init; }
    public string Excerpt { get; init; } = string.Empty;
    public DateTime PublishDate { get; init; }
}
