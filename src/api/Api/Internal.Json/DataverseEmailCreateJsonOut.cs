using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal sealed record class DataverseEmailCreateJsonOut
{
    [JsonPropertyName("activityid")]
    public Guid ActivityId { get; init; }
}