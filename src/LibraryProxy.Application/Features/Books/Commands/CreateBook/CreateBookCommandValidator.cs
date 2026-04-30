namespace LibraryProxy.Application.Features.Books.Commands.CreateBook;

public sealed class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Book.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Book.PageCount)
            .GreaterThan(0);

        RuleFor(x => x.Book.PublishDate)
            .NotEmpty();
    }
}
