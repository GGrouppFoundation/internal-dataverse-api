using System;

namespace GarageGroup.Infra;

public readonly record struct DataverseEmailCreateOut
{
    public DataverseEmailCreateOut(Guid emailId) 
        => 
        EmailId = emailId;

    public Guid EmailId { get; }
}
