namespace LibraryProxy.Application.Features.Authors.Commands.DeleteAuthor;

public sealed class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
{
    public DeleteAuthorCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}
