#nullable enable

using System.Text.Json.Serialization;

namespace GGroupp.Infra
{
    internal sealed record DataverseFailureInfoJson
    {
        [JsonPropertyName("code")]
        public string? Code { get; init; }

        [JsonPropertyName("message")]
        public string? Message { get; init; }
    }
}