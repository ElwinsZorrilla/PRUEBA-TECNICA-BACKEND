using LibraryProxy.Application.Common.Services;

namespace LibraryProxy.Application.Features.Authors.Queries.GetAuthorById;

public sealed class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorEnrichmentService _enrichment;

    public GetAuthorByIdQueryHandler(
        IAuthorRepository authorRepository,
        IBookRepository bookRepository,
        IAuthorEnrichmentService enrichment)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
        _enrichment = enrichment;
    }

    public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(request.Id, cancellationToken);
        if (author is null)
            throw new NotFoundException(nameof(Author), request.Id);

        var allAuthors = (await _authorRepository.GetAllAsync(cancellationToken)).ToList();
        var allBooks = (await _bookRepository.GetAllAsync(cancellationToken)).ToList();
        return _enrichment.Enrich(author, allBooks, allAuthors);
    }
}
