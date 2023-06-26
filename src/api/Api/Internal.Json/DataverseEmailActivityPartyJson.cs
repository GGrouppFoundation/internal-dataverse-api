using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal sealed record class DataverseEmailActivityPartyJson
{
    [JsonPropertyName("partyid_systemuser@odata.bind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SystemUserIdParty { get; init; }

    [JsonPropertyName("partyid_account@odata.bind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccountIdParty { get; init; }

    [JsonPropertyName("partyid_contact@odata.bind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContactIdParty { get; init; }

    [JsonPropertyName("participationtypemask")]
    public int? ParticipationTypeMask { get; init; }

    [JsonPropertyName("addressused")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AddressUsed { get; init; }
}