using System;

namespace GGroupp.Infra;

public sealed record class DataverseEmailSendIn
{
    public DataverseEmailSendIn(Guid emailId)
    {
        EmailId = emailId;
        Subject = string.Empty;
        Body = string.Empty;
        Sender = new(string.Empty);
        Recipients = new FlatArray<DataverseEmailRecipient>();
    }
    
    public DataverseEmailSendIn(
        string subject,
        string body,
        DataverseEmailSender sender,
        FlatArray<DataverseEmailRecipient> recipients)
    {
        Subject = subject ?? string.Empty;
        Body = body ?? string.Empty;
        Sender = sender;
        Recipients = recipients;
        EmailId = null;
    }
    
    public Guid? EmailId { get; }

    public string Subject { get; }
    
    public string Body { get; }
    
    public DataverseEmailSender Sender { get; }
    
    public FlatArray<DataverseEmailRecipient> Recipients { get; }
}