using System.Text.Json.Serialization;

namespace GGroupp.Infra.Dataverse.Api.Test;

internal sealed record class StubEntitySetJsonOut
{
    [JsonPropertyName("value")]
    public StubResponseJson[]? Value { get; init; }

    [JsonPropertyName("@odata.nextLink")]
    public string? NextLink { get; init; }
}