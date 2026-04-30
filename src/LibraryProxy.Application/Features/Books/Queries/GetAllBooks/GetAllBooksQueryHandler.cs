namespace LibraryProxy.Application.Features.Books.Queries.GetAllBooks;

public sealed class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public GetAllBooksQueryHandler(IBookRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }
}
