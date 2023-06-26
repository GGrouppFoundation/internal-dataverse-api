using System;

namespace GarageGroup.Infra;

public sealed record class DataverseEmailCreateOut
{
    public DataverseEmailCreateOut(Guid emailId) 
        => 
        EmailId = emailId;
    
    public Guid EmailId { get; }
}
