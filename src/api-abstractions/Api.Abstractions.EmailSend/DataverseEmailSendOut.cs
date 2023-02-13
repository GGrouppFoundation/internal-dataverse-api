using System;

namespace GGroupp.Infra;

public sealed record class DataverseEmailSendOut
{
    
    public DataverseEmailSendOut(Guid emailId) 
        => 
        EmailId = emailId;
    
    public Guid EmailId { get; }
}