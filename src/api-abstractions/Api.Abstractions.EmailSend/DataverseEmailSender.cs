namespace GGroupp.Infra;

public record DataverseEmailSender
{
    public DataverseEmailSender(string email)
    {
        SenderEmail = email ?? string.Empty;
        SenderMember = default;
    }

    public DataverseEmailSender(DataverseEmailMember senderMember)
    {
        SenderEmail = string.Empty;
        SenderMember = senderMember;
    }

    public string SenderEmail { get; }
    
    public DataverseEmailMember? SenderMember { get; }
}