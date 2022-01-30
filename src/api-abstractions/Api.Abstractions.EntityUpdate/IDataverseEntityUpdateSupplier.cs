using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDataverseEntityUpdateSupplier
{
    ValueTask<Result<DataverseEntityUpdateOut<TOutJson>, Failure<DataverseFailureCode>>> UpdateEntityAsync<TInJson, TOutJson>(
        DataverseEntityUpdateIn<TInJson> input, CancellationToken cancellationToken = default);
}