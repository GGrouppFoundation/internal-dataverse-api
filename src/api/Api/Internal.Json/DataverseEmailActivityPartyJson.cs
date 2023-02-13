using System.Text.Json.Serialization;

namespace GGroupp.Infra;

public record DataverseEmailActivityPartyJson
{
    [JsonPropertyName("partyid_systemuser@odata.bind")]
    public string? SystemUserIdParty {get; init;}
    
    [JsonPropertyName("partyid_account@odata.bind")]
    public string? AccountIdParty {get; init;}

    [JsonPropertyName("partyid_contact@odata.bind")]
    public string? ContactIdParty {get; init;}
    
    [JsonPropertyName("participationtypemask")]
    public int? ParticipationTypeMask {get; init;}

    [JsonPropertyName("addressused")]   
    public string? AddressUsed {get; init;}
}