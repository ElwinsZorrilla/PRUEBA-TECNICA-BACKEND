namespace LibraryProxy.Application.Features.Books.Commands.UpdateBook;

public sealed record UpdateBookCommand(int Id, UpdateBookDto Book) : IRequest<BookDto>;
