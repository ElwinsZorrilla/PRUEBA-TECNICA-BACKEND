using FluentValidation.Results;
using LibraryProxy.Application.Common.Behaviors;
using LibraryProxy.Application.Features.Books.Queries.GetAllBooks;

namespace LibraryProxy.Application.Tests.Common.Behaviors;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Should_CallNext_When_NoValidatorsRegistered()
    {
        var behavior = new ValidationBehavior<GetAllBooksQuery, IEnumerable<BookDto>>(
            Enumerable.Empty<IValidator<GetAllBooksQuery>>());

        var nextCalled = false;
        RequestHandlerDelegate<IEnumerable<BookDto>> next = (ct) =>
        {
            nextCalled = true;
            return Task.FromResult(Enumerable.Empty<BookDto>());
        };

        await behavior.Handle(new GetAllBooksQuery(), next, CancellationToken.None);

        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Should_ThrowValidationException_When_ValidationFails()
    {
        var validatorMock = new Mock<IValidator<GetAllBooksQuery>>();
        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetAllBooksQuery>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Property", "Error message") }));

        var behavior = new ValidationBehavior<GetAllBooksQuery, IEnumerable<BookDto>>(
            new[] { validatorMock.Object });

        RequestHandlerDelegate<IEnumerable<BookDto>> next = (ct) =>
            Task.FromResult(Enumerable.Empty<BookDto>());

        await FluentActions.Invoking(() => behavior.Handle(new GetAllBooksQuery(), next, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Should_CallNext_When_ValidationPasses()
    {
        var validatorMock = new Mock<IValidator<GetAllBooksQuery>>();
        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetAllBooksQuery>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var behavior = new ValidationBehavior<GetAllBooksQuery, IEnumerable<BookDto>>(
            new[] { validatorMock.Object });

        var nextCalled = false;
        RequestHandlerDelegate<IEnumerable<BookDto>> next = (ct) =>
        {
            nextCalled = true;
            return Task.FromResult(Enumerable.Empty<BookDto>());
        };

        await behavior.Handle(new GetAllBooksQuery(), next, CancellationToken.None);

        nextCalled.Should().BeTrue();
    }
}
