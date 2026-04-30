namespace LibraryProxy.Application.Features.Authors.Commands.UpdateAuthor;

public sealed record UpdateAuthorCommand(int Id, UpdateAuthorDto Author) : IRequest<AuthorDto>;
