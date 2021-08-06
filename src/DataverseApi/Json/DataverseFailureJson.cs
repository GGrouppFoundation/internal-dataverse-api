#nullable enable

using System.Text.Json.Serialization;

namespace GGroupp
{
    internal sealed record DataverseFailureJson
    {
        [JsonPropertyName("error")]
        public DataverseFailureInfoJson? Error { get; init; }
    }
}