using LibraryProxy.Application.Common.Services;

namespace LibraryProxy.Application.Features.Authors.Queries.GetAllAuthors;

public sealed class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<AuthorDto>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorEnrichmentService _enrichment;

    public GetAllAuthorsQueryHandler(
        IAuthorRepository authorRepository,
        IBookRepository bookRepository,
        IAuthorEnrichmentService enrichment)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
        _enrichment = enrichment;
    }

    public async Task<IEnumerable<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = (await _authorRepository.GetAllAsync(cancellationToken)).ToList();
        var books = (await _bookRepository.GetAllAsync(cancellationToken)).ToList();
        return _enrichment.EnrichMany(authors, books, authors);
    }
}
