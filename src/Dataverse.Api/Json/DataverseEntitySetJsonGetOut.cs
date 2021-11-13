using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DataverseEntitySetJsonGetOut<TEntityJson>
{
    [JsonPropertyName("value")]
    public TEntityJson[]? Value { get; init; }
}