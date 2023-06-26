using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IDataverseEntityDeleteSupplier
{
    ValueTask<Result<Unit, Failure<DataverseFailureCode>>> DeleteEntityAsync(
        DataverseEntityDeleteIn input, CancellationToken cancellationToken = default);
}