using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IDataverseSearchSupplier
{
    ValueTask<Result<DataverseSearchOut, Failure<DataverseFailureCode>>> SearchAsync(
        DataverseSearchIn input, CancellationToken cancellationToken = default);
}