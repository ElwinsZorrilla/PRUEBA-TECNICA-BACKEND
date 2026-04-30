using AutoMapper;
using LibraryProxy.Domain.Common;
using LibraryProxy.Domain.Entities;
using LibraryProxy.Domain.Interfaces;
using MediatR;

namespace LibraryProxy.Application.Features.Authors.Queries.GetAuthorsPagedQuery;

public class GetAuthorsPagedQueryHandler : IRequestHandler<GetAuthorsPagedQuery, CursorPage<AuthorDto>>
{
    private readonly IAuthorRepository _repository;
    private readonly IMapper _mapper;

    public GetAuthorsPagedQueryHandler(IAuthorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CursorPage<AuthorDto>> Handle(GetAuthorsPagedQuery request, CancellationToken cancellationToken)
    {
        var pageSize = Math.Max(1, Math.Min(request.PageSize, 100));
        var allAuthors = await _repository.GetAllAsync(cancellationToken);

        var items = new List<Author>(allAuthors);
        items.Sort((a, b) => a.Id.CompareTo(b.Id));

        var startIndex = 0;
        if (!string.IsNullOrEmpty(request.Cursor))
        {
            try
            {
                var decodedBytes = Convert.FromBase64String(request.Cursor);
                var cursorId = int.Parse(System.Text.Encoding.UTF8.GetString(decodedBytes));
                startIndex = items.FindIndex(a => a.Id > cursorId);
                if (startIndex == -1)
                {
                    return new CursorPage<AuthorDto>
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

        var mappedItems = _mapper.Map<List<AuthorDto>>(pageItems);

        return new CursorPage<AuthorDto>
        {
            Items = mappedItems.AsReadOnly(),
            NextCursor = nextCursor,
            PreviousCursor = previousCursor,
            PageSize = pageSize
        };
    }
}
