using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface IDataverseChangeSetExecuteSupplier
{
    ValueTask<Result<DataverseChangeSetExecuteOut<TOut>, Failure<DataverseFailureCode>>> ExecuteChangeSetAsync<TIn, TOut>(
        DataverseChangeSetExecuteIn<TIn> input, CancellationToken cancellationToken = default)
        where TIn : notnull;

    ValueTask<Result<Unit, Failure<DataverseFailureCode>>> ExecuteChangeSetAsync<TIn>(
        DataverseChangeSetExecuteIn<TIn> input, CancellationToken cancellationToken = default)
        where TIn : notnull;
}