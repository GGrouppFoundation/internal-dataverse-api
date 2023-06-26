using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Dataverse.Api.Test;

internal sealed record class StubFailureJson
{
    [JsonPropertyName("error")]
    public StubFailureInfoJson? Failure { get; init; }

    [JsonPropertyName("_error")]
    public StubErrorInfoJson? Error { get; init; }

    [JsonPropertyName("ErrorCode")]
    public string? ErrorCode { get; init; }

    [JsonPropertyName("Message")]
    public string? Message { get; init; }

    [JsonPropertyName("ExceptionMessage")]
    public string? ExceptionMessage { get; init; }
}