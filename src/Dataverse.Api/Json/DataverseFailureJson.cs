using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal readonly record struct DataverseFailureJson
{
    [JsonPropertyName("error")]
    public DataverseFailureInfoJson Error { get; init; }
}