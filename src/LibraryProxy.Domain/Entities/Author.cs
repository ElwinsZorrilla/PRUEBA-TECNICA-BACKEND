namespace LibraryProxy.Domain.Entities;

public class Author
{
    public int Id { get; set; }
    public int IdBook { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
