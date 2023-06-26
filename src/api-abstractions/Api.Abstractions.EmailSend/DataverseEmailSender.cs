namespace GarageGroup.Infra;

public sealed record class DataverseEmailSender
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