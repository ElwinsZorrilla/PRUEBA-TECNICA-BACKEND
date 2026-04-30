using LibraryProxy.Application.Features.Authors.Queries.GetAllAuthors;

namespace LibraryProxy.Application.Tests.Features.Authors.Queries;

public class GetAllAuthorsQueryHandlerTests
{
    private readonly Mock<IAuthorRepository> _repositoryMock;
    private readonly IMapper _mapper;

    public GetAllAuthorsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IAuthorRepository>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappingProfile>()).CreateMapper();
    }

    [Fact]
    public async Task Should_ReturnAllAuthors_When_RepositoryHasData()
    {
        var authors = new List<Author>
        {
            new() { Id = 1, IdBook = 1, FirstName = "John", LastName = "Doe" },
            new() { Id = 2, IdBook = 2, FirstName = "Jane", LastName = "Smith" }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(authors);

        var handler = new GetAllAuthorsQueryHandler(_repositoryMock.Object, _mapper);
        var result = await handler.Handle(new GetAllAuthorsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.First().FirstName.Should().Be("John");
    }

    [Fact]
    public async Task Should_ReturnEmptyList_When_RepositoryIsEmpty()
    {
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Author>());

        var handler = new GetAllAuthorsQueryHandler(_repositoryMock.Object, _mapper);
        var result = await handler.Handle(new GetAllAuthorsQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}
