namespace LibraryProxy.Application.Features.Authors.Commands.CreateAuthor;

public sealed record CreateAuthorCommand(CreateAuthorDto Author) : IRequest<AuthorDto>;
