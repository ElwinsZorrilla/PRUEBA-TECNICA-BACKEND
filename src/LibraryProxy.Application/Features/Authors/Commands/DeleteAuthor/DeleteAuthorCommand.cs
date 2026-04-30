namespace LibraryProxy.Application.Features.Authors.Commands.DeleteAuthor;

public sealed record DeleteAuthorCommand(int Id) : IRequest<Unit>;
