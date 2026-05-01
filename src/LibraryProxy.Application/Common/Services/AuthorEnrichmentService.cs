namespace LibraryProxy.Application.Common.Services;

public interface IAuthorEnrichmentService
{
    public AuthorDto Enrich(Author author, IReadOnlyList<Book> allBooks, IReadOnlyList<Author> allAuthors);
    public IReadOnlyList<AuthorDto> EnrichMany(IEnumerable<Author> authors, IReadOnlyList<Book> allBooks, IReadOnlyList<Author> allAuthors);
}

public sealed class AuthorEnrichmentService : IAuthorEnrichmentService
{
    public AuthorDto Enrich(Author author, IReadOnlyList<Book> allBooks, IReadOnlyList<Author> allAuthors)
    {
        var countByBook = BuildCountMap(allAuthors);
        var bookById = BuildTitleMap(allBooks);
        return MapAuthor(author, bookById, countByBook);
    }

    public IReadOnlyList<AuthorDto> EnrichMany(IEnumerable<Author> authors, IReadOnlyList<Book> allBooks, IReadOnlyList<Author> allAuthors)
    {
        var countByBook = BuildCountMap(allAuthors);
        var bookById = BuildTitleMap(allBooks);
        return authors.Select(a => MapAuthor(a, bookById, countByBook)).ToList();
    }

    private static Dictionary<int, int> BuildCountMap(IEnumerable<Author> authors) =>
        authors.GroupBy(a => a.IdBook).ToDictionary(g => g.Key, g => g.Count());

    private static Dictionary<int, string> BuildTitleMap(IEnumerable<Book> books) =>
        books.GroupBy(b => b.Id).ToDictionary(g => g.Key, g => g.First().Title);

    private static AuthorDto MapAuthor(Author author, Dictionary<int, string> bookById, Dictionary<int, int> countByBook) =>
        new()
        {
            Id = author.Id,
            IdBook = author.IdBook,
            FirstName = author.FirstName,
            LastName = author.LastName,
            BookTitle = bookById.TryGetValue(author.IdBook, out var t) ? t : null,
            BookCount = countByBook.TryGetValue(author.IdBook, out var c) ? c : 0
        };
}
