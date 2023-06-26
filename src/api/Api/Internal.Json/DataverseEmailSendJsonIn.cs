using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal sealed record class DataverseEmailSendJsonIn
{
    [JsonPropertyName("IssueSend")]
    public bool? IssueSend { get; init;}
}