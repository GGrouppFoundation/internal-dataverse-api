#nullable enable

using System.Text.Json.Serialization;

namespace GGroupp.Infra
{
    internal sealed record DataverseEntityJsonGetOut<TEntityJson>
    {
        [JsonPropertyName("value")]
        public TEntityJson? Value { get; init; }
    }
}