using System.Text.Json.Serialization;

namespace GGroupp.Infra.Dataverse.Api.Test;

internal sealed record class StubWhoAmIOutJson
{
    [JsonPropertyName("BusinessUnitId")]
    public string? BusinessUnitId { get; init; }

    [JsonPropertyName("UserId")]
    public string? UserId { get; init; }

    [JsonPropertyName("OrganizationId")]
    public string? OrganizationId { get; init; }
}