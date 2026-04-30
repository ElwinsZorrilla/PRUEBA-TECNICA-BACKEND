namespace LibraryProxy.Application.Features.Books.Commands.UpdateBook;

public sealed class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Book.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Book.PageCount)
            .GreaterThan(0);
    }
}
