using System;
using System.Globalization;

namespace GarageGroup.Infra;

public sealed record class DataversePrimaryKey : IDataverseEntityKey
{
    public DataversePrimaryKey(Guid entityId)
        =>
        Value = entityId.ToString("D", CultureInfo.InvariantCulture);

    public string Value { get; }
}