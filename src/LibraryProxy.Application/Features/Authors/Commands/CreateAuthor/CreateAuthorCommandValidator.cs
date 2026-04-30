namespace LibraryProxy.Application.Features.Authors.Commands.CreateAuthor;

public sealed class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.Author.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Author.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Author.IdBook)
            .GreaterThan(0);
    }
}
