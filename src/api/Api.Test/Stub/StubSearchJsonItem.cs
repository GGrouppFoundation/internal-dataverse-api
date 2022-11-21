using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GGroupp.Infra.Dataverse.Api.Test;

internal sealed record class StubSearchJsonItem
{
    [JsonPropertyName("@search.score")]
    public double SearchScore { get; init; }

    [JsonPropertyName("@search.entityname")]
    public string? EntityName { get; init; }

    [JsonPropertyName("@search.objectid")]
    public string? ObjectId { get; init; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }
}