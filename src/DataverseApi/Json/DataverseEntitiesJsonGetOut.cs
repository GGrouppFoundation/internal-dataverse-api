#nullable enable

using System.Text.Json.Serialization;

namespace GGroupp
{
    internal sealed record DataverseEntitiesJsonGetOut<TEntityJson>
    {
        [JsonPropertyName("value")]
        public TEntityJson[]? Value { get; init; }
    }
}