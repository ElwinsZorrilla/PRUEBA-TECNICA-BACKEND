using LibraryProxy.Application.Features.Books.Queries.GetAllBooks;

namespace LibraryProxy.Application.Tests.Features.Books.Queries;

public class GetAllBooksQueryHandlerTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly IMapper _mapper;

    public GetAllBooksQueryHandlerTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<BookMappingProfile>()).CreateMapper();
    }

    [Fact]
    public async Task Should_ReturnAllBooks_When_RepositoryHasData()
    {
        var books = new List<Book>
        {
            new() { Id = 1, Title = "Book One", PageCount = 100, PublishDate = DateTime.UtcNow },
            new() { Id = 2, Title = "Book Two", PageCount = 200, PublishDate = DateTime.UtcNow }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        var handler = new GetAllBooksQueryHandler(_repositoryMock.Object, _mapper);
        var result = await handler.Handle(new GetAllBooksQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.First().Title.Should().Be("Book One");
    }

    [Fact]
    public async Task Should_ReturnEmptyList_When_RepositoryIsEmpty()
    {
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Book>());

        var handler = new GetAllBooksQueryHandler(_repositoryMock.Object, _mapper);
        var result = await handler.Handle(new GetAllBooksQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}
