using System;

namespace GGroupp.Infra;

public sealed record class DataverseEmailCreateIn
{
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

    public string Subject { get; }
    
    public string Body { get; }
    
    public DataverseEmailSender Sender { get; }
    
    public FlatArray<DataverseEmailRecipient> Recipients { get; }
}