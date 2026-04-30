namespace LibraryProxy.Application.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string Excerpt { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
}

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string Excerpt { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
}

public class UpdateBookDto : CreateBookDto { }
