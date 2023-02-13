using System;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DataverseEmailCreateJsonIn
{
    [JsonPropertyName("description")]
    public string? Description { get; init;}

    [JsonPropertyName("subject")]
    public string? Subject { get; init; }
    
    [JsonPropertyName("email_activity_parties")]
    public FlatArray<DataverseEmailActivityPartyJson> ActivityParties { get; init; }
}