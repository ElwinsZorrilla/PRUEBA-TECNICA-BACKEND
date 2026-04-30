namespace LibraryProxy.Application.Features.Authors.Queries.GetAuthorById;

public sealed record GetAuthorByIdQuery(int Id) : IRequest<AuthorDto>;
