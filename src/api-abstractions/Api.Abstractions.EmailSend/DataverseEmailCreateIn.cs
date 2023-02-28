using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GGroupp.Infra;

public sealed record class DataverseEmailCreateIn
{
    public DataverseEmailCreateIn(
        string subject,
        string body,
        DataverseEmailSender sender,
        FlatArray<DataverseEmailRecipient> recipients,
        FlatArray<KeyValuePair<string, JsonElement>> extensionData = default)
    {
        Subject = subject ?? string.Empty;
        Body = body ?? string.Empty;
        Sender = sender;
        Recipients = recipients;
        ExtensionData = extensionData;
    }

    public string Subject { get; }
    
    public string Body { get; }
    
    public DataverseEmailSender Sender { get; }
    
    public FlatArray<DataverseEmailRecipient> Recipients { get; }

    public FlatArray<KeyValuePair<string, JsonElement>> ExtensionData { get; }
}