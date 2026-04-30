using System.Net;
using System.Net.Http.Json;
using AutoMapper;
using LibraryProxy.Infrastructure.ExternalApi;
using LibraryProxy.Infrastructure.ExternalApi.Models;

namespace LibraryProxy.Infrastructure.Repositories;

public sealed class AuthorRepository : IAuthorRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMapper _mapper;

    public AuthorRepository(
        IHttpClientFactory httpClientFactory,
        IMapper mapper)
    {
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
    }

    private HttpClient CreateClient() => _httpClientFactory.CreateClient("FakeRestAPI");

    public async Task<IEnumerable<Author>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.GetAsync("Authors", cancellationToken);
        response.EnsureSuccessStatusCode();
        var apiModels = await response.Content
            .ReadFromJsonAsync<IEnumerable<AuthorApiModel>>(FakeRestApiJsonOptions.Default, cancellationToken)
            ?? [];
        return _mapper.Map<IEnumerable<Author>>(apiModels);
    }

    public async Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.GetAsync($"Authors/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var apiModel = await response.Content
            .ReadFromJsonAsync<AuthorApiModel>(FakeRestApiJsonOptions.Default, cancellationToken);

        return apiModel is null ? null : _mapper.Map<Author>(apiModel);
    }

    public async Task<Author> CreateAsync(Author author, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("Authors", author, cancellationToken);
        response.EnsureSuccessStatusCode();
        var apiModel = await response.Content
            .ReadFromJsonAsync<AuthorApiModel>(FakeRestApiJsonOptions.Default, cancellationToken)
            ?? throw new ExternalApiException("FakeRestAPI returned empty body on POST /Authors", (int)response.StatusCode);
        return _mapper.Map<Author>(apiModel);
    }

    public async Task<Author> UpdateAsync(int id, Author author, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.PutAsJsonAsync($"Authors/{id}", author, FakeRestApiJsonOptions.Default, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException(nameof(Author), id);

        response.EnsureSuccessStatusCode();

        var apiModel = await response.Content
            .ReadFromJsonAsync<AuthorApiModel>(FakeRestApiJsonOptions.Default, cancellationToken)
            ?? throw new ExternalApiException("FakeRestAPI returned empty body on PUT /Authors", (int)response.StatusCode);

        return _mapper.Map<Author>(apiModel);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"Authors/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException(nameof(Author), id);

        response.EnsureSuccessStatusCode();
    }
}
