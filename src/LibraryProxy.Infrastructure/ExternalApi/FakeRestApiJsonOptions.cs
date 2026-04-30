using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibraryProxy.Infrastructure.ExternalApi;

public static class FakeRestApiJsonOptions
{
    public static readonly JsonSerializerOptions Default = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
