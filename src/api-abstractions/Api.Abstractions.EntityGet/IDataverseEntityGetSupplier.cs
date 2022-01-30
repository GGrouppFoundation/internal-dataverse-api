using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDataverseEntityGetSupplier
{
    ValueTask<Result<DataverseEntityGetOut<TJson>, Failure<DataverseFailureCode>>> GetEntityAsync<TJson>(
        DataverseEntityGetIn input, CancellationToken cancellationToken = default);
}