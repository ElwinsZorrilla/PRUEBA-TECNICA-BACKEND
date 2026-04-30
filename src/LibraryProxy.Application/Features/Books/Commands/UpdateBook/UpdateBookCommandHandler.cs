namespace LibraryProxy.Application.Features.Books.Commands.UpdateBook;

public sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public UpdateBookCommandHandler(IBookRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existing is null)
            throw new NotFoundException(nameof(Book), request.Id);

        var book = _mapper.Map<Book>(request.Book);
        var updated = await _repository.UpdateAsync(request.Id, book, cancellationToken);
        return _mapper.Map<BookDto>(updated);
    }
}
