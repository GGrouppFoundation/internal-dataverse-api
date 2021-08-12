#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra
{
    public interface IDataverseEntityUpdateSupplier
    {
        ValueTask<Result<DataverseEntityUpdateOut<TResponseJson>, Failure<int>>> UpdateEntityAsync<TRequestJson, TResponseJson>(
            DataverseEntityUpdateIn<TRequestJson> input, CancellationToken cancellationToken = default);
    }
}