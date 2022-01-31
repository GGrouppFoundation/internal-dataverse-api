using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntityUpdateOut<TOutJson>, Failure<DataverseFailureCode>>> UpdateEntityAsync<TInJson, TOutJson>(
        DataverseEntityUpdateIn<TInJson> input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEntityUpdateOut<TOutJson>>(cancellationToken);
        }

        return InnerUpdateEntityAsync<TInJson, TOutJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntityUpdateOut<TOutJson>, Failure<DataverseFailureCode>>> InnerUpdateEntityAsync<TInJson, TOutJson>(
        DataverseEntityUpdateIn<TInJson> input, CancellationToken cancellationToken)
    {
        using var httpClient = await DataverseHttpHelper.InternalCreateHttpClientAsync(
                messageHandler,
                configuration,
                apiVersion: ApiVersionData,
                apiType: ApiTypeData)
            .ConfigureAwait(false);

        var entitiyUpdateUrl = BuildEntityUpdateUrl(input);

        using var content = DataverseHttpHelper.InternalBuildRequestJsonBody(input.EntityData);

        var response = await httpClient.PatchAsync(entitiyUpdateUrl, content, cancellationToken).ConfigureAwait(false);
        var result = await response.InternalReadDataverseResultAsync<TOutJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(e => new DataverseEntityUpdateOut<TOutJson>(e));
    }

    private static string BuildEntityUpdateUrl<TInJson>(DataverseEntityUpdateIn<TInJson> input)
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