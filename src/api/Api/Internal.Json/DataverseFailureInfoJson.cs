using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DataverseFailureInfoJson
{
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }
}