using System.Net;
using System.Net.Http.Json;
using AutoMapper;
using LibraryProxy.Infrastructure.ExternalApi;
using LibraryProxy.Infrastructure.ExternalApi.Models;

namespace LibraryProxy.Infrastructure.Repositories;

public sealed class BookRepository : IBookRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMapper _mapper;

    public BookRepository(
        IHttpClientFactory httpClientFactory,
        IMapper mapper)
    {
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
    }

    private HttpClient CreateClient() => _httpClientFactory.CreateClient("FakeRestAPI");

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.GetAsync("Books", cancellationToken);
        response.EnsureSuccessStatusCode();
        var apiModels = await response.Content
            .ReadFromJsonAsync<IEnumerable<BookApiModel>>(FakeRestApiJsonOptions.Default, cancellationToken)
            ?? [];
        return _mapper.Map<IEnumerable<Book>>(apiModels);
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.GetAsync($"Books/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var apiModel = await response.Content
            .ReadFromJsonAsync<BookApiModel>(FakeRestApiJsonOptions.Default, cancellationToken);

        return apiModel is null ? null : _mapper.Map<Book>(apiModel);
    }

    public async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("Books", book, cancellationToken);
        response.EnsureSuccessStatusCode();
        var apiModel = await response.Content
            .ReadFromJsonAsync<BookApiModel>(FakeRestApiJsonOptions.Default, cancellationToken)
            ?? throw new ExternalApiException("FakeRestAPI returned empty body on POST /Books", (int)response.StatusCode);
        return _mapper.Map<Book>(apiModel);
    }

    public async Task<Book> UpdateAsync(int id, Book book, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"Books/{id}", book, FakeRestApiJsonOptions.Default, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException(nameof(Book), id);

        response.EnsureSuccessStatusCode();

        var apiModel = await response.Content
            .ReadFromJsonAsync<BookApiModel>(FakeRestApiJsonOptions.Default, cancellationToken)
            ?? throw new ExternalApiException("FakeRestAPI returned empty body on PUT /Books", (int)response.StatusCode);

        return _mapper.Map<Book>(apiModel);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"Books/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException(nameof(Book), id);

        response.EnsureSuccessStatusCode();
    }
}
