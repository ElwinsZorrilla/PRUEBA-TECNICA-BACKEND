namespace LibraryProxy.Application.Features.Authors.Queries.GetAllAuthors;

public sealed class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<AuthorDto>>
{
    private readonly IAuthorRepository _repository;
    private readonly IMapper _mapper;

    public GetAllAuthorsQueryHandler(IAuthorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }
}
