using LibraryProxy.Domain.Common;
using MediatR;

namespace LibraryProxy.Application.Features.Books.Queries.GetBooksPagedQuery;

public record GetBooksPagedQuery(string? Cursor, int PageSize = 10) : IRequest<CursorPage<BookDto>>;
