using LibraryProxy.Application.Features.Authors.Commands.CreateAuthor;

namespace LibraryProxy.Application.Tests.Features.Authors.Commands;

public class CreateAuthorCommandValidatorTests
{
    private readonly CreateAuthorCommandValidator _validator;

    public CreateAuthorCommandValidatorTests()
    {
        _validator = new CreateAuthorCommandValidator();
    }

    [Fact]
    public async Task Should_Pass_When_ValidCommand()
    {
        var command = new CreateAuthorCommand(new CreateAuthorDto
        {
            FirstName = "John",
            LastName = "Doe",
            IdBook = 1
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Fail_When_FirstNameEmpty()
    {
        var command = new CreateAuthorCommand(new CreateAuthorDto
        {
            FirstName = string.Empty,
            LastName = "Doe",
            IdBook = 1
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("FirstName"));
    }

    [Fact]
    public async Task Should_Fail_When_LastNameEmpty()
    {
        var command = new CreateAuthorCommand(new CreateAuthorDto
        {
            FirstName = "John",
            LastName = string.Empty,
            IdBook = 1
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("LastName"));
    }

    [Fact]
    public async Task Should_Fail_When_IdBookZero()
    {
        var command = new CreateAuthorCommand(new CreateAuthorDto
        {
            FirstName = "John",
            LastName = "Doe",
            IdBook = 0
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("IdBook"));
    }

    [Fact]
    public async Task Should_Fail_When_NamesTooLong()
    {
        var command = new CreateAuthorCommand(new CreateAuthorDto
        {
            FirstName = new string('A', 101),
            LastName = new string('B', 101),
            IdBook = 1
        });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
