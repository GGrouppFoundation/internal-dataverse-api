using System;

namespace GGroupp.Infra;

public sealed record class DataverseEmailCreateOut
{
    public DataverseEmailCreateOut(Guid emailId) 
        => 
        EmailId = emailId;
    
    public Guid EmailId { get; }
}
