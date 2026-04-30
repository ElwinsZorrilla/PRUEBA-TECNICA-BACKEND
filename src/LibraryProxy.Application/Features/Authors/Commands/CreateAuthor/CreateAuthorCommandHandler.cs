namespace LibraryProxy.Application.Features.Authors.Commands.CreateAuthor;

public sealed class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IAuthorRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateAuthorCommandHandler(IAuthorRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = _mapper.Map<Author>(request.Author);
        var created = await _repository.CreateAsync(author, cancellationToken);
        var dto = _mapper.Map<AuthorDto>(created);
        await _mediator.Publish(new AuthorCreatedNotification(dto), cancellationToken);
        return dto;
    }
}
