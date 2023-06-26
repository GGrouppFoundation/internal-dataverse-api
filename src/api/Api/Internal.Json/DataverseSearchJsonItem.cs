using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal sealed record class DataverseSearchJsonItem
{
    [JsonPropertyName("@search.score")]
    public double SearchScore { get; init; }

    [JsonPropertyName("@search.entityname")]
    public string? EntityName { get; init; }

    [JsonPropertyName("@search.objectid")]
    public Guid ObjectId { get; init; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; init; }
}
