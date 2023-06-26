using System;

namespace GarageGroup.Infra;

public readonly record struct DataverseWhoAmIOut
{
    public DataverseWhoAmIOut(Guid businessUnitId, Guid userId, Guid organizationId)
    {
        BusinessUnitId = businessUnitId;
        UserId = userId;
        OrganizationId = organizationId;
    }

    public Guid BusinessUnitId { get; }

    public Guid UserId { get; }

    public Guid OrganizationId { get; }
}