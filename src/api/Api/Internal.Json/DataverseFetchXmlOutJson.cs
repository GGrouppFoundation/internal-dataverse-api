using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal readonly record struct  DataverseFetchXmlOutJson<TOutJson>
{
    [JsonPropertyName("value")]
    public FlatArray<TOutJson> Value { get; init; }

    [JsonPropertyName("@Microsoft.Dynamics.CRM.fetchxmlpagingcookie")]
    public string? PagingCookie { get; init; }
}