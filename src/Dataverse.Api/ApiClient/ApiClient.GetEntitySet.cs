using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntitySetGetOut<TEntityJson>, Failure<int>>> GetEntitySetAsync<TEntityJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        return cancellationToken.IsCancellationRequested
            ? ValueTask.FromCanceled<Result<DataverseEntitySetGetOut<TEntityJson>, Failure<int>>>(cancellationToken)
            : InnerGetEntitySetAsync<TEntityJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntitySetGetOut<TEntityJson>, Failure<int>>> InnerGetEntitySetAsync<TEntityJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken)
    {
        var httpClient = await DataverseHttpHelper
            .InternalCreateHttpClientAsync(
                messageHandler,
                configurationProvider.Invoke(),
                apiVersion: ApiVersionData,
                apiType: ApiTypeData)
            .ConfigureAwait(false); 

        var entitiesGetUrl = BuildEntitySetGetUrl(input);

        var response = await httpClient.GetAsync(entitiesGetUrl, cancellationToken).ConfigureAwait(false);
        var result = await response.InternalReadDataverseResultAsync<DataverseEntitySetJsonGetOut<TEntityJson>>(cancellationToken);

        return result.MapSuccess(e => new DataverseEntitySetGetOut<TEntityJson>(e?.Value));
    }
    
    private static string BuildEntitySetGetUrl(DataverseEntitySetGetIn input)
        =>
        Pipeline.Pipe(
            new Dictionary<string, string>
            { 
                ["$select"] = QueryParametersBuilder.InternalBuildOdataParameterValue(input.SelectFields),
                ["$filter"] = input.Filter
            })
        .Pipe<Dictionary<string, string>,IReadOnlyCollection<KeyValuePair<string, string>>>(
            parameters =>
            {
                if (input.Top.HasValue)
                {
                    parameters.Add("$top", input.Top.Value.ToString(CultureInfo.InvariantCulture));
                }
                return parameters;
            })
        .Pipe(
            QueryParametersBuilder.InternalBuildQueryString)
        .Pipe(
            queryString => input.EntityPluralName + queryString);
}