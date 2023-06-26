using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal interface IDataverseHttpApi
{
    ValueTask<Result<TOut?, Failure<DataverseFailureCode>>> InvokeAsync<TIn, TOut>(
        DataverseHttpRequest<TIn> request, CancellationToken cacncellationToken)
        where TIn : notnull;
}