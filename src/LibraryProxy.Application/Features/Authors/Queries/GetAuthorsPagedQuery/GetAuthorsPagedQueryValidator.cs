using FluentValidation;

namespace LibraryProxy.Application.Features.Authors.Queries.GetAuthorsPagedQuery;

public class GetAuthorsPagedQueryValidator : AbstractValidator<GetAuthorsPagedQuery>
{
    public GetAuthorsPagedQueryValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("PageSize must not exceed 100");

        RuleFor(x => x.Cursor)
            .Must(BeValidBase64).When(x => !string.IsNullOrEmpty(x.Cursor))
            .WithMessage("Cursor must be a valid base64 string");
    }

    private static bool BeValidBase64(string? cursor)
    {
        if (string.IsNullOrEmpty(cursor))
            return true;

        try
        {
            Convert.FromBase64String(cursor);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
