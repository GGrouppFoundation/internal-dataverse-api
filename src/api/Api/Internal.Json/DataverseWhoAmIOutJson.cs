using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal readonly record struct DataverseWhoAmIOutJson
{
    [JsonPropertyName("BusinessUnitId")]
    public Guid BusinessUnitId { get; init; }

    [JsonPropertyName("UserId")]
    public Guid UserId { get; init; }

    [JsonPropertyName("OrganizationId")]
    public Guid OrganizationId { get; init; }
}