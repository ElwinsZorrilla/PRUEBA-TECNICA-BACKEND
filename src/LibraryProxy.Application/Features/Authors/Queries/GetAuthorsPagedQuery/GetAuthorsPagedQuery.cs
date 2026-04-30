using LibraryProxy.Domain.Common;
using MediatR;

namespace LibraryProxy.Application.Features.Authors.Queries.GetAuthorsPagedQuery;

public record GetAuthorsPagedQuery(string? Cursor, int PageSize = 10) : IRequest<CursorPage<AuthorDto>>;
