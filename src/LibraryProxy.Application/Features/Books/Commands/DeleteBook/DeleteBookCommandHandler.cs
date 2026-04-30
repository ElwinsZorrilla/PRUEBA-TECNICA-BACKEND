namespace LibraryProxy.Application.Features.Books.Commands.DeleteBook;

public sealed class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
{
    private readonly IBookRepository _repository;

    public DeleteBookCommandHandler(IBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existing is null)
            throw new NotFoundException(nameof(Book), request.Id);

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
