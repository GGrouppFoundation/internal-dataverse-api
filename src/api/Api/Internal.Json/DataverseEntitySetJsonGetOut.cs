using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal readonly record struct DataverseEntitySetJsonGetOut<TOutJson>
{
    [JsonPropertyName("value")]
    public TOutJson[]? Value { get; init; }

    [JsonPropertyName("@odata.nextLink")]
    public string? NextLink { get; init; }
}