#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra
{
    public interface IDataverseEntityGetSupplier
    {
        ValueTask<Result<DataverseEntityGetOut<TEntityJson>, Failure<int>>> GetEntityAsync<TEntityJson>(
            DataverseEntityGetIn input, CancellationToken cancellationToken = default);
    }
}