using System;

namespace GGroupp.Infra;

public readonly record struct DataverseEmailSendOut
{
    public DataverseEmailSendOut(Guid emailId) 
        => 
        EmailId = emailId;
    
    public Guid EmailId { get; }
}