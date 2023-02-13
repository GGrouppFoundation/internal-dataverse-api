using System;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DataverseEmailSendJsonIn
{
    [JsonPropertyName("IssueSend")]
    public bool? IssueSend { get; init;}
}