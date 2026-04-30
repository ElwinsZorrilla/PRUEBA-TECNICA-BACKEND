using LibraryProxy.Application.Features.Books.Commands.DeleteBook;

namespace LibraryProxy.Application.Tests.Features.Books.Commands;

public class DeleteBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _repositoryMock;

    public DeleteBookCommandHandlerTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
    }

    [Fact]
    public async Task Should_DeleteBook_When_BookExists()
    {
        var book = new Book { Id = 1, Title = "Book to Delete", PageCount = 100 };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        _repositoryMock
            .Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new DeleteBookCommandHandler(_repositoryMock.Object);
        var act = async () => await handler.Handle(new DeleteBookCommand(1), CancellationToken.None);

        await act.Should().NotThrowAsync();
        _repositoryMock.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_ThrowNotFoundException_When_BookDoesNotExist()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        var handler = new DeleteBookCommandHandler(_repositoryMock.Object);

        await FluentActions.Invoking(() => handler.Handle(new DeleteBookCommand(99), CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();

        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
