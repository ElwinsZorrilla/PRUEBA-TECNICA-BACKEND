namespace LibraryProxy.Application.Features.Authors.Commands.DeleteAuthor;

public sealed class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Unit>
{
    private readonly IAuthorRepository _repository;

    public DeleteAuthorCommandHandler(IAuthorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existing is null)
            throw new NotFoundException(nameof(Author), request.Id);

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
