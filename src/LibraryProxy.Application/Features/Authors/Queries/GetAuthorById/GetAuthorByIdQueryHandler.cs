namespace LibraryProxy.Application.Features.Authors.Queries.GetAuthorById;

public sealed class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    private readonly IAuthorRepository _repository;
    private readonly IMapper _mapper;

    public GetAuthorByIdQueryHandler(IAuthorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (author is null)
            throw new NotFoundException(nameof(Author), request.Id);

        return _mapper.Map<AuthorDto>(author);
    }
}
