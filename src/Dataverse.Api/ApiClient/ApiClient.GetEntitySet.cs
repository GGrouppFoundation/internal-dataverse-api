using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntitySetGetOut<TEntityJson>, Failure<int>>> GetEntitySetAsync<TEntityJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken = default)
        =>
        (input, cancellationToken.IsCancellationRequested) switch
        {
            (null, _) => throw new ArgumentNullException(nameof(input)),
            (_, true) => ValueTask.FromCanceled<Result<DataverseEntitySetGetOut<TEntityJson>, Failure<int>>>(cancellationToken),
            _ => InternalGetEntitySetAsync<TEntityJson>(input, cancellationToken)
        };

    private async ValueTask<Result<DataverseEntitySetGetOut<TEntityJson>, Failure<int>>> InternalGetEntitySetAsync<TEntityJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken)
    {
        var httpClient = await DataverseHttpHelper.CreateHttpClientAsync(messageHandler, clientConfiguration); 
        var entitiesGetUrl = BuildEntitySetGetUrl(input);

        var response = await httpClient.GetAsync(entitiesGetUrl, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<DataverseEntitySetJsonGetOut<TEntityJson>>(cancellationToken);

        return result.MapSuccess(e => new DataverseEntitySetGetOut<TEntityJson>(e?.Value));
    }
    
    private static string BuildEntitySetGetUrl(DataverseEntitySetGetIn input)
        =>
        Pipeline.Pipe<IReadOnlyCollection<KeyValuePair<string, string>>>(
            new Dictionary<string, string>
            { 
                ["$select"] = QueryParametersBuilder.BuildOdataParameterValue(input.SelectFields),
                ["$filter"] = input.Filter
            })
        .Pipe(
            QueryParametersBuilder.BuildQueryString)
        .Pipe(
            queryString => input.EntitySetName + queryString);
}