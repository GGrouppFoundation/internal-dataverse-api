using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal readonly record struct DataverseFailureInfoJson
{
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }
}