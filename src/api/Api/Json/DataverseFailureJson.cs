using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal readonly record struct DataverseFailureJson
{
    [JsonPropertyName("error")]
    public DataverseFailureInfoJson? Failure { get; init; }

    [JsonPropertyName("_error")]
    public DataverseErrorInfoJson? Error { get; init; }

    [JsonPropertyName("ErrorCode")]
    public string? ErrorCode { get; init; }

    [JsonPropertyName("Message")]
    public string? Message { get; init; }

    [JsonPropertyName("ExceptionMessage")]
    public string? ExceptionMessage { get; init; }
}