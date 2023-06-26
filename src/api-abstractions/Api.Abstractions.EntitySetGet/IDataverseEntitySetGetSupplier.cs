using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IDataverseEntitySetGetSupplier
{
    ValueTask<Result<DataverseEntitySetGetOut<TJson>, Failure<DataverseFailureCode>>> GetEntitySetAsync<TJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken = default);
}