using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DataverseEntitySetJsonGetOut<TOutJson>
{
    [JsonPropertyName("value")]
    public TOutJson[]? Value { get; init; }
}