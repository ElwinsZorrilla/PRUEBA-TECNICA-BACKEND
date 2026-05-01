using LibraryProxy.Application.Common.Services;
using LibraryProxy.Domain.Common;

namespace LibraryProxy.Application.Features.Authors.Queries.GetAuthorsPagedQuery;

public class GetAuthorsPagedQueryHandler : IRequestHandler<GetAuthorsPagedQuery, CursorPage<AuthorDto>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorEnrichmentService _enrichment;

    public GetAuthorsPagedQueryHandler(
        IAuthorRepository authorRepository,
        IBookRepository bookRepository,
        IAuthorEnrichmentService enrichment)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
        _enrichment = enrichment;
    }

    public async Task<CursorPage<AuthorDto>> Handle(GetAuthorsPagedQuery request, CancellationToken cancellationToken)
    {
        var pageSize = Math.Max(1, Math.Min(request.PageSize, 100));
        var allAuthors = (await _authorRepository.GetAllAsync(cancellationToken)).ToList();
        var allBooks = (await _bookRepository.GetAllAsync(cancellationToken)).ToList();

        allAuthors.Sort((a, b) => a.Id.CompareTo(b.Id));

        var startIndex = 0;
        if (!string.IsNullOrEmpty(request.Cursor))
        {
            try
            {
                var decodedBytes = Convert.FromBase64String(request.Cursor);
                var cursorId = int.Parse(System.Text.Encoding.UTF8.GetString(decodedBytes));
                startIndex = allAuthors.FindIndex(a => a.Id > cursorId);
                if (startIndex == -1)
                {
                    return new CursorPage<AuthorDto>
                    {
                        Items = [],
                        NextCursor = null,
                        PreviousCursor = null,
                        PageSize = pageSize
                    };
                }
            }
            catch
            {
                throw new ArgumentException("Invalid cursor format");
            }
        }

        var hasMore = startIndex + pageSize < allAuthors.Count;
        var pageItems = allAuthors.Skip(startIndex).Take(pageSize).ToList();

        var nextCursor = hasMore && pageItems.Count > 0
            ? Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pageItems[^1].Id.ToString()))
            : null;

        var previousCursor = startIndex > 0 && pageItems.Count > 0
            ? Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pageItems[0].Id.ToString()))
            : null;

        var mappedItems = _enrichment.EnrichMany(pageItems, allBooks, allAuthors);

        return new CursorPage<AuthorDto>
        {
            Items = mappedItems,
            NextCursor = nextCursor,
            PreviousCursor = previousCursor,
            PageSize = pageSize
        };
    }
}
