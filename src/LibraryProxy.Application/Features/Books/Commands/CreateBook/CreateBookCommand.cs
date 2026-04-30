namespace LibraryProxy.Application.Features.Books.Commands.CreateBook;

public sealed record CreateBookCommand(CreateBookDto Book) : IRequest<BookDto>;
