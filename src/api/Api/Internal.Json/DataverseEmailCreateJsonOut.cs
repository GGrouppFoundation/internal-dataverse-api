using System;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DataverseEmailCreateJsonOut
{
    [JsonPropertyName("activityid")]
    public Guid ActivityId { get; init; }
}