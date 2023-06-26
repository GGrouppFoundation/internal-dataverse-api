using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal sealed record class DataverseFailureInfoJson
{
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }
}