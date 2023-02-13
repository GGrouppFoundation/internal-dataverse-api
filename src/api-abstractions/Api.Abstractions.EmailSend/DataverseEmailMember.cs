using System;

namespace GGroupp.Infra;

public sealed record class DataverseEmailMember
{
    public DataverseEmailMember(
        Guid memberId,
        DataverseEmailMemberType memberType)
    {
        MemberId = memberId;
        MemberType = memberType;
    }
    
    public Guid MemberId { get; }
    
    public DataverseEmailMemberType MemberType { get; }
}