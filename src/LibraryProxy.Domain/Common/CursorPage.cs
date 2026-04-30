namespace LibraryProxy.Domain.Common;

public class CursorPage<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public string? NextCursor { get; init; }
    public string? PreviousCursor { get; init; }
    public int PageSize { get; init; }
}
