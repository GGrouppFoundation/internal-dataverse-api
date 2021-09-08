using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDataverseEntityCreateSupplier
{
    ValueTask<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>> CreateEntityAsync<TRequestJson, TResponseJson>(
        DataverseEntityCreateIn<TRequestJson> input, CancellationToken cancellationToken = default);
}