using LibraryProxy.Application.Common.Services;
using LibraryProxy.Application.Features.Authors.Queries.GetAllAuthors;

namespace LibraryProxy.Application.Tests.Features.Authors.Queries;

public class GetAllAuthorsQueryHandlerTests
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly IAuthorEnrichmentService _enrichment;

    public GetAllAuthorsQueryHandlerTests()
    {
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _bookRepositoryMock = new Mock<IBookRepository>();
        _enrichment = new AuthorEnrichmentService();
    }

    [Fact]
    public async Task Should_ReturnAllAuthors_When_RepositoryHasData()
    {
        var authors = new List<Author>
        {
            new() { Id = 1, IdBook = 1, FirstName = "John", LastName = "Doe" },
            new() { Id = 2, IdBook = 1, FirstName = "Mary", LastName = "Johnson" },
            new() { Id = 3, IdBook = 2, FirstName = "Jane", LastName = "Smith" }
        };
        var books = new List<Book>
        {
            new() { Id = 1, Title = "Book One" },
            new() { Id = 2, Title = "Book Two" }
        };

        _authorRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(authors);
        _bookRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        var handler = new GetAllAuthorsQueryHandler(_authorRepositoryMock.Object, _bookRepositoryMock.Object, _enrichment);
        var result = (await handler.Handle(new GetAllAuthorsQuery(), CancellationToken.None)).ToList();

        result.Should().HaveCount(3);
        result[0].FirstName.Should().Be("John");
        result[0].BookTitle.Should().Be("Book One");
        result[0].BookCount.Should().Be(2);
        result[2].BookTitle.Should().Be("Book Two");
        result[2].BookCount.Should().Be(1);
    }

    [Fact]
    public async Task Should_ReturnEmptyList_When_RepositoryIsEmpty()
    {
        _authorRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Author>());
        _bookRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Book>());

        var handler = new GetAllAuthorsQueryHandler(_authorRepositoryMock.Object, _bookRepositoryMock.Object, _enrichment);
        var result = await handler.Handle(new GetAllAuthorsQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}
