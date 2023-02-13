namespace GGroupp.Infra;

public sealed record class DataverseEmailRecipient
{
    public DataverseEmailRecipient(string email, DataverseEmailRecipientType emailRecipientType)
    {
        SenderRecipientEmail = email;
        EmailMember = default;
        EmailRecipientType = emailRecipientType;
    }

    public DataverseEmailRecipient(
        DataverseEmailMember emailMember,
        DataverseEmailRecipientType emailRecipientType)
    {
        SenderRecipientEmail = string.Empty;
        EmailMember = emailMember;
        EmailRecipientType = emailRecipientType;
    }

    public string SenderRecipientEmail { get; }
    
    public DataverseEmailMember? EmailMember { get; }
    
    public DataverseEmailRecipientType EmailRecipientType { get; }
}