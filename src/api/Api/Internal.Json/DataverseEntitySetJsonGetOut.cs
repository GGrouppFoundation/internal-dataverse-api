using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal readonly record struct DataverseEntitySetJsonGetOut<TOutJson>
{
    [JsonPropertyName("value")]
    public FlatArray<TOutJson> Value { get; init; }

    [JsonPropertyName("@odata.nextLink")]
    public string? NextLink { get; init; }
}