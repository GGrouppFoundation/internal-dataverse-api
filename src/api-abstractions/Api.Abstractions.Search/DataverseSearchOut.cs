using System;

namespace GarageGroup.Infra;

public sealed record class DataverseSearchOut
{
    public DataverseSearchOut(int totalRecordCount, FlatArray<DataverseSearchItem> value)
    {
        TotalRecordCount = totalRecordCount;
        Value = value;
    }

    public int TotalRecordCount { get; }

    public FlatArray<DataverseSearchItem> Value { get; }
}