using System;
using System.Collections.Generic;
using System.Text.Json;
#if NET7_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GarageGroup.Infra;

public sealed record class DataverseEmailCreateIn
{
#if NET7_0_OR_GREATER
    [SetsRequiredMembers]
#endif
    public DataverseEmailCreateIn(
        string subject,
        string body,
        DataverseEmailSender sender,
        FlatArray<DataverseEmailRecipient> recipients,
        FlatArray<KeyValuePair<string, JsonElement>> extensionData)
    {
        Subject = subject ?? string.Empty;
        Body = body ?? string.Empty;
        Sender = sender;
        Recipients = recipients;
        ExtensionData = extensionData;
    }

#if NET7_0_OR_GREATER
    [SetsRequiredMembers]
#endif
    public DataverseEmailCreateIn(
        string subject,
        string body,
        DataverseEmailSender sender,
        FlatArray<DataverseEmailRecipient> recipients)
    {
        Subject = subject ?? string.Empty;
        Body = body ?? string.Empty;
        Sender = sender;
        Recipients = recipients;
    }

#if NET7_0_OR_GREATER
    public DataverseEmailCreateIn(
        string subject,
        string body,
        DataverseEmailSender sender)
    {
        Subject = subject ?? string.Empty;
        Body = body ?? string.Empty;
        Sender = sender;
    }
#endif

    public string Subject { get; }

    public string Body { get; }

    public DataverseEmailSender Sender { get; }

#if NET7_0_OR_GREATER
    public required FlatArray<DataverseEmailRecipient> Recipients { get; init; }
#else
    public FlatArray<DataverseEmailRecipient> Recipients { get; }
#endif

    public FlatArray<KeyValuePair<string, JsonElement>> ExtensionData { get; init; }
}