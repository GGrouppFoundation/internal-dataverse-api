using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntityGetOut<TEntityJson>, Failure<int>>> GetEntityAsync<TEntityJson>(
        DataverseEntityGetIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        return cancellationToken.IsCancellationRequested
            ? ValueTask.FromCanceled<Result<DataverseEntityGetOut<TEntityJson>, Failure<int>>>(cancellationToken)
            : InnerGetEntityAsync<TEntityJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntityGetOut<TEntityJson>, Failure<int>>> InnerGetEntityAsync<TEntityJson>(
        DataverseEntityGetIn input, CancellationToken cancellationToken)
    {
        var httpClient = await DataverseHttpHelper
            .InternalCreateHttpClientAsync(
                messageHandler,
                configurationProvider.Invoke(),
                apiVersion: ApiVersionData,
                apiType: ApiTypeData)
            .ConfigureAwait(false); 

        var entitiesGetUrl = BuildEntityGetUrl(input);

        var response = await httpClient.GetAsync(entitiesGetUrl, cancellationToken).ConfigureAwait(false);
        var result = await response.InternalReadDataverseResultAsync<TEntityJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(e => new DataverseEntityGetOut<TEntityJson>(e));
    }

    private static string BuildEntityGetUrl(DataverseEntityGetIn input)
        =>
        Pipeline.Pipe<IReadOnlyCollection<KeyValuePair<string, string>>>(
            new Dictionary<string, string>
            {
                ["$select"] = QueryParametersBuilder.InternalBuildOdataParameterValue(input.SelectFields)
            })
        .Pipe(
            QueryParametersBuilder.InternalBuildQueryString)
        .Pipe(
            queryString => $"{input.EntityPluralName}({input.EntityKey.Value}){queryString}");
}