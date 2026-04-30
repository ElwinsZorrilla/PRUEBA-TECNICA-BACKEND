using LibraryProxy.Application.Features.Books.Commands.CreateBook;

namespace LibraryProxy.Application.Tests.Features.Books.Commands;

public class CreateBookCommandValidatorTests
{
    private readonly CreateBookCommandValidator _validator;

    public CreateBookCommandValidatorTests()
    {
        _validator = new CreateBookCommandValidator();
    }

    [Fact]
    public async Task Should_Pass_When_ValidCommand()
    {
        var command = new CreateBookCommand(new CreateBookDto
        {
            Title = "Valid Title",
            PageCount = 100,
            PublishDate = DateTime.UtcNow
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Fail_When_TitleEmpty()
    {
        var command = new CreateBookCommand(new CreateBookDto
        {
            Title = string.Empty,
            PageCount = 100,
            PublishDate = DateTime.UtcNow
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Title"));
    }

    [Fact]
    public async Task Should_Fail_When_TitleTooLong()
    {
        var command = new CreateBookCommand(new CreateBookDto
        {
            Title = new string('A', 201),
            PageCount = 100,
            PublishDate = DateTime.UtcNow
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Title"));
    }

    [Fact]
    public async Task Should_Fail_When_PageCountZero()
    {
        var command = new CreateBookCommand(new CreateBookDto
        {
            Title = "Valid Title",
            PageCount = 0,
            PublishDate = DateTime.UtcNow
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("PageCount"));
    }

    [Fact]
    public async Task Should_Fail_When_PublishDateEmpty()
    {
        var command = new CreateBookCommand(new CreateBookDto
        {
            Title = "Valid Title",
            PageCount = 100,
            PublishDate = default
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("PublishDate"));
    }
}
