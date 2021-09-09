using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record DataverseSearchJsonItem
{
    [JsonPropertyName("@search.score")]
    public double SearchScore { get; init; }

    [JsonPropertyName("@search.entityname")]
    public string? EntityName { get; init; }

    [JsonPropertyName("@search.objectid")]
    public Guid ObjectId { get; init; }
}
