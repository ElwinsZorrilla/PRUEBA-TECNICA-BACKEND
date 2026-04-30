namespace LibraryProxy.Application.DTOs;

public class AuthorDto
{
    public int Id { get; set; }
    public int IdBook { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class CreateAuthorDto
{
    public int IdBook { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class UpdateAuthorDto : CreateAuthorDto { }
