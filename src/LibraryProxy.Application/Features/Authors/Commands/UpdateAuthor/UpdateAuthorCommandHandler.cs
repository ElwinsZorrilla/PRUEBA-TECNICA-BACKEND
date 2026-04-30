namespace LibraryProxy.Application.Features.Authors.Commands.UpdateAuthor;

public sealed class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto>
{
    private readonly IAuthorRepository _repository;
    private readonly IMapper _mapper;

    public UpdateAuthorCommandHandler(IAuthorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AuthorDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existing is null)
            throw new NotFoundException(nameof(Author), request.Id);

        var author = _mapper.Map<Author>(request.Author);
        var updated = await _repository.UpdateAsync(request.Id, author, cancellationToken);
        return _mapper.Map<AuthorDto>(updated);
    }
}
