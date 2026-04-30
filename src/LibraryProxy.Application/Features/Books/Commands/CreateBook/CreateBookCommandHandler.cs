namespace LibraryProxy.Application.Features.Books.Commands.CreateBook;

public sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateBookCommandHandler(IBookRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = _mapper.Map<Book>(request.Book);
        var created = await _repository.CreateAsync(book, cancellationToken);
        var dto = _mapper.Map<BookDto>(created);
        await _mediator.Publish(new BookCreatedNotification(dto), cancellationToken);
        return dto;
    }
}
