#nullable enable

using System.Text.Json.Serialization;

namespace GGroupp.Infra
{
    internal sealed record DataverseEntityGetJsonGetOut<TEntityJson>
    {
        [JsonPropertyName("value")]
        public TEntityJson[]? Value { get; init; }
    }
}