using LibraryProxy.Application.Features.Authors.Commands.CreateAuthor;
using LibraryProxy.Application.Common.Notifications;

namespace LibraryProxy.Application.Tests.Features.Authors.Commands;

public class CreateAuthorCommandHandlerTests
{
    private readonly Mock<IAuthorRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly IMapper _mapper;

    public CreateAuthorCommandHandlerTests()
    {
        _repositoryMock = new Mock<IAuthorRepository>();
        _mediatorMock = new Mock<IMediator>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappingProfile>()).CreateMapper();
    }

    [Fact]
    public async Task Should_CreateAuthor_And_ReturnDto_When_CommandIsValid()
    {
        var dto = new CreateAuthorDto { FirstName = "John", LastName = "Doe", IdBook = 1 };
        var createdAuthor = new Author { Id = 1, FirstName = "John", LastName = "Doe", IdBook = 1 };

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdAuthor);

        var handler = new CreateAuthorCommandHandler(_repositoryMock.Object, _mapper, _mediatorMock.Object);
        var result = await handler.Handle(new CreateAuthorCommand(dto), CancellationToken.None);

        result.Should().NotBeNull();
        result.FirstName.Should().Be("John");
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task Should_CallRepositoryOnce_When_CommandIsExecuted()
    {
        var dto = new CreateAuthorDto { FirstName = "John", LastName = "Doe", IdBook = 1 };
        var createdAuthor = new Author { Id = 1, FirstName = "John", LastName = "Doe", IdBook = 1 };

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdAuthor);

        var handler = new CreateAuthorCommandHandler(_repositoryMock.Object, _mapper, _mediatorMock.Object);
        await handler.Handle(new CreateAuthorCommand(dto), CancellationToken.None);

        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_PublishAuthorCreatedNotification_When_AuthorIsCreated()
    {
        var dto = new CreateAuthorDto { FirstName = "John", LastName = "Doe", IdBook = 1 };
        var createdAuthor = new Author { Id = 1, FirstName = "John", LastName = "Doe", IdBook = 1 };

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdAuthor);

        var handler = new CreateAuthorCommandHandler(_repositoryMock.Object, _mapper, _mediatorMock.Object);
        await handler.Handle(new CreateAuthorCommand(dto), CancellationToken.None);

        _mediatorMock.Verify(
            m => m.Publish(It.IsAny<AuthorCreatedNotification>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
