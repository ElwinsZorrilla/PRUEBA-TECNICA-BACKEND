using LibraryProxy.Application.Features.Books.Commands.CreateBook;
using LibraryProxy.Application.Common.Notifications;

namespace LibraryProxy.Application.Tests.Features.Books.Commands;

public class CreateBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly IMapper _mapper;

    public CreateBookCommandHandlerTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _mediatorMock = new Mock<IMediator>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<BookMappingProfile>()).CreateMapper();
    }

    [Fact]
    public async Task Should_CreateBook_And_ReturnDto_When_CommandIsValid()
    {
        var dto = new CreateBookDto { Title = "New Book", PageCount = 100, PublishDate = DateTime.UtcNow };
        var createdBook = new Book { Id = 1, Title = "New Book", PageCount = 100, PublishDate = dto.PublishDate };

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdBook);

        var handler = new CreateBookCommandHandler(_repositoryMock.Object, _mapper, _mediatorMock.Object);
        var result = await handler.Handle(new CreateBookCommand(dto), CancellationToken.None);

        result.Should().NotBeNull();
        result.Title.Should().Be("New Book");
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task Should_CallRepositoryOnce_When_CommandIsExecuted()
    {
        var dto = new CreateBookDto { Title = "New Book", PageCount = 100, PublishDate = DateTime.UtcNow };
        var createdBook = new Book { Id = 1, Title = "New Book", PageCount = 100, PublishDate = dto.PublishDate };

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdBook);

        var handler = new CreateBookCommandHandler(_repositoryMock.Object, _mapper, _mediatorMock.Object);
        await handler.Handle(new CreateBookCommand(dto), CancellationToken.None);

        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_PublishBookCreatedNotification_When_BookIsCreated()
    {
        var dto = new CreateBookDto { Title = "New Book", PageCount = 100, PublishDate = DateTime.UtcNow };
        var createdBook = new Book { Id = 1, Title = "New Book", PageCount = 100, PublishDate = dto.PublishDate };

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdBook);

        var handler = new CreateBookCommandHandler(_repositoryMock.Object, _mapper, _mediatorMock.Object);
        await handler.Handle(new CreateBookCommand(dto), CancellationToken.None);

        _mediatorMock.Verify(
            m => m.Publish(It.IsAny<BookCreatedNotification>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
