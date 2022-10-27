using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseSearchOut
{
    public DataverseSearchOut(
        int totalRecordCount,
        [AllowNull] FlatArray<DataverseSearchItem> value)
    {
        TotalRecordCount = totalRecordCount;
        Value = value ?? FlatArray.Empty<DataverseSearchItem>();
    }

    public int TotalRecordCount { get; }

    public FlatArray<DataverseSearchItem> Value { get; }
}