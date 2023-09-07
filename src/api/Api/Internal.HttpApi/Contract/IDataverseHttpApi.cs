using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal interface IDataverseHttpApi
{
    ValueTask<Result<DataverseJsonResponse, Failure<DataverseFailureCode>>> SendJsonAsync(
        DataverseJsonRequest request, CancellationToken cacncellationToken);

    ValueTask<Result<DataverseChangeSetResponse, Failure<DataverseFailureCode>>> SendChangeSetAsync(
        DataverseChangeSetRequest request, CancellationToken cacncellationToken);
}