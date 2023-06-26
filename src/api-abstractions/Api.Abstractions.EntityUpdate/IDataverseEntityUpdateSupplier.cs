using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IDataverseEntityUpdateSupplier
{
    ValueTask<Result<DataverseEntityUpdateOut<TOutJson>, Failure<DataverseFailureCode>>> UpdateEntityAsync<TInJson, TOutJson>(
        DataverseEntityUpdateIn<TInJson> input, CancellationToken cancellationToken = default)
        where TInJson : notnull;

    ValueTask<Result<Unit, Failure<DataverseFailureCode>>> UpdateEntityAsync<TInJson>(
        DataverseEntityUpdateIn<TInJson> input, CancellationToken cancellationToken = default)
        where TInJson : notnull;
}