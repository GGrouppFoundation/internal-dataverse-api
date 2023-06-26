using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IDataverseEntityCreateSupplier
{
    ValueTask<Result<DataverseEntityCreateOut<TOutJson>, Failure<DataverseFailureCode>>> CreateEntityAsync<TInJson, TOutJson>(
        DataverseEntityCreateIn<TInJson> input, CancellationToken cancellationToken = default)
        where TInJson : notnull;

    ValueTask<Result<Unit, Failure<DataverseFailureCode>>> CreateEntityAsync<TInJson>(
        DataverseEntityCreateIn<TInJson> input, CancellationToken cancellationToken = default)
        where TInJson : notnull;
}