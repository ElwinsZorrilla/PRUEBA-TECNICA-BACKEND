namespace LibraryProxy.Application.Features.Authors.Commands.UpdateAuthor;

public sealed class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

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
