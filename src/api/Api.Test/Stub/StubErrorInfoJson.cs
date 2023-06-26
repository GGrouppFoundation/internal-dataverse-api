using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Dataverse.Api.Test;

internal sealed record class StubErrorInfoJson
{
    [JsonPropertyName("Code")]
    public string? Code { get; init; }

    [JsonPropertyName("Description")]
    public string? Description { get; init; }
}