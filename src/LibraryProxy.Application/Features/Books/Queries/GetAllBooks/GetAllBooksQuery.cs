namespace LibraryProxy.Application.Features.Books.Queries.GetAllBooks;

public sealed record GetAllBooksQuery : IRequest<IEnumerable<BookDto>>;
