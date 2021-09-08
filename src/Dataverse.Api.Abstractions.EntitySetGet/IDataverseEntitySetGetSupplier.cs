using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDataverseEntitySetGetSupplier
{
    ValueTask<Result<DataverseEntitySetGetOut<TEntityJson>, Failure<int>>> GetEntitySetAsync<TEntityJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken = default);
}