namespace LibraryProxy.Application.Features.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(int Id) : IRequest<BookDto>;
