using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal sealed record class DataverseErrorInfoJson
{
    [JsonPropertyName("Code")]
    public string? Code { get; init; }

    [JsonPropertyName("Description")]
    public string? Description { get; init; }
}