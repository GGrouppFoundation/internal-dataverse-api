#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp
{
    public interface IDataverseEntitiesGetSupplier
    {
        ValueTask<Result<DataverseEntitiesGetOut<TEntityJson>, Failure<int>>> GetEntitiesAsync<TEntityJson>(
            DataverseEntitiesGetIn input, CancellationToken cancellationToken = default);
    }
}