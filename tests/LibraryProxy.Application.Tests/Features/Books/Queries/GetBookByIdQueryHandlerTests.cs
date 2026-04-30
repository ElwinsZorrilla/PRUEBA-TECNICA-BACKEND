using LibraryProxy.Application.Features.Books.Queries.GetBookById;

namespace LibraryProxy.Application.Tests.Features.Books.Queries;

public class GetBookByIdQueryHandlerTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<BookMappingProfile>()).CreateMapper();
    }

    [Fact]
    public async Task Should_ReturnBook_When_BookExists()
    {
        var book = new Book { Id = 1, Title = "Test Book", PageCount = 150, PublishDate = DateTime.UtcNow };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        var handler = new GetBookByIdQueryHandler(_repositoryMock.Object, _mapper);
        var result = await handler.Handle(new GetBookByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Test Book");
    }

    [Fact]
    public async Task Should_ThrowNotFoundException_When_BookDoesNotExist()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        var handler = new GetBookByIdQueryHandler(_repositoryMock.Object, _mapper);

        await FluentActions.Invoking(() => handler.Handle(new GetBookByIdQuery(99), CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }
}
