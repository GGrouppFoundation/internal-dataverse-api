using System;

namespace GarageGroup.Infra;

public readonly record struct DataverseEmailSendOut
{
    public DataverseEmailSendOut(Guid emailId) 
        => 
        EmailId = emailId;

    public Guid EmailId { get; }
}