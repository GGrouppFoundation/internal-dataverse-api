using System;

namespace GarageGroup.Infra;

public readonly record struct DataverseChangeSetExecuteIn<TIn>
    where TIn : notnull
{
    public DataverseChangeSetExecuteIn(FlatArray<IDataverseTransactableIn<TIn>> requests)
        =>
        Requests = requests;

    public FlatArray<IDataverseTransactableIn<TIn>> Requests { get; }
}