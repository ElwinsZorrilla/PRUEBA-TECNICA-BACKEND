namespace LibraryProxy.Infrastructure.ExternalApi.Models;

public sealed class AuthorApiModel
{
    public int Id { get; init; }
    public int IdBook { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}
