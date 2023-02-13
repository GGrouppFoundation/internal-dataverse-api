using System;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

public sealed record class DataverseEmailCreateJsonOut
{
    [JsonPropertyName("activityid")]
    public Guid ActivityId { get; init; }
}