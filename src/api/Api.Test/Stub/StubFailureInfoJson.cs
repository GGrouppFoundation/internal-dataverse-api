using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Dataverse.Api.Test;

internal sealed record class StubFailureInfoJson
{
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }
}