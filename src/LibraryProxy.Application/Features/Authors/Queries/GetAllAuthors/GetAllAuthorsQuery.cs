namespace LibraryProxy.Application.Features.Authors.Queries.GetAllAuthors;

public sealed record GetAllAuthorsQuery : IRequest<IEnumerable<AuthorDto>>;
