using AutoMapper;
using LibraryProxy.Domain.Common;
using LibraryProxy.Domain.Entities;
using LibraryProxy.Domain.Interfaces;
using MediatR;

namespace LibraryProxy.Application.Features.Books.Queries.GetBooksPagedQuery;

public class GetBooksPagedQueryHandler : IRequestHandler<GetBooksPagedQuery, CursorPage<BookDto>>
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public GetBooksPagedQueryHandler(IBookRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CursorPage<BookDto>> Handle(GetBooksPagedQuery request, CancellationToken cancellationToken)
    {
        var pageSize = Math.Max(1, Math.Min(request.PageSize, 100));
        var allBooks = await _repository.GetAllAsync(cancellationToken);

        var items = new List<Book>(allBooks);
        items.Sort((a, b) => a.Id.CompareTo(b.Id));

        var startIndex = 0;
        if (!string.IsNullOrEmpty(request.Cursor))
        {
            try
            {
                var decodedBytes = Convert.FromBase64String(request.Cursor);
                var cursorId = int.Parse(System.Text.Encoding.UTF8.GetString(decodedBytes));
                startIndex = items.FindIndex(b => b.Id > cursorId);
                if (startIndex == -1)
                {
                    return new CursorPage<BookDto>
                    {
                        Items = [],
                        NextCursor = null,
                        PreviousCursor = null,
                        PageSize = pageSize
                    };
                }
            }
            catch
            {
                throw new ArgumentException("Invalid cursor format");
            }
        }

        var hasMore = startIndex + pageSize < items.Count;
        var pageItems = items.Skip(startIndex).Take(pageSize).ToList();

        var nextCursor = hasMore && pageItems.Count > 0
            ? Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pageItems[^1].Id.ToString()))
            : null;

        var previousCursor = startIndex > 0 && pageItems.Count > 0
            ? Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pageItems[0].Id.ToString()))
            : null;

        var mappedItems = _mapper.Map<List<BookDto>>(pageItems);

        return new CursorPage<BookDto>
        {
            Items = mappedItems.AsReadOnly(),
            NextCursor = nextCursor,
            PreviousCursor = previousCursor,
            PageSize = pageSize
        };
    }
}
