using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IDataverseWhoAmISupplier
{
    ValueTask<Result<DataverseWhoAmIOut, Failure<DataverseFailureCode>>> WhoAmIAsync(
        CancellationToken cancellationToken = default)
        =>
        WhoAmIAsync(default, cancellationToken);

    ValueTask<Result<DataverseWhoAmIOut, Failure<DataverseFailureCode>>> WhoAmIAsync(
        Unit input, CancellationToken cancellationToken = default);
}