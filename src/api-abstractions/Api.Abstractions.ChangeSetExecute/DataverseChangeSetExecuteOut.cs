using System;

namespace GarageGroup.Infra;

public readonly record struct DataverseChangeSetExecuteOut<TOut>
{
    public DataverseChangeSetExecuteOut(FlatArray<TOut?> values)
        =>
        Values = values;

    public FlatArray<TOut?> Values { get; }
}