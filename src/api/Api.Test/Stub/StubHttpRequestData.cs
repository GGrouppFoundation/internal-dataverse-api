using System.Collections.Generic;
using System.Net.Http;

namespace GGroupp.Infra.Dataverse.Api.Test;

internal sealed record class StubHttpRequestData
{
    public required HttpMethod Method { get; init; }

    public required string? RequestUrl { get; init; }

    public FlatArray<KeyValuePair<string, string>> Headers { get; init; }

    public string? Content { get; init; }
}