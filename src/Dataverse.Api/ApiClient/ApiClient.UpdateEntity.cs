using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntityUpdateOut<TResponseJson>, Failure<int>>> UpdateEntityAsync<TRequestJson, TResponseJson>(
        DataverseEntityUpdateIn<TRequestJson> input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        return cancellationToken.IsCancellationRequested
            ? ValueTask.FromCanceled<Result<DataverseEntityUpdateOut<TResponseJson>, Failure<int>>>(cancellationToken)
            : InnerUpdateEntityAsync<TRequestJson, TResponseJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntityUpdateOut<TResponseJson>, Failure<int>>> InnerUpdateEntityAsync<TRequestJson, TResponseJson>(
        DataverseEntityUpdateIn<TRequestJson> input, CancellationToken cancellationToken)
    {
        using var httpClient = await DataverseHttpHelper.InternalCreateHttpClientAsync(
                messageHandler,
                configurationProvider.Invoke(),
                apiVersion: ApiVersionData,
                apiType: ApiTypeData)
            .ConfigureAwait(false);

        var entitiyUpdateUrl = BuildEntityUpdateUrl(input);

        using var content = DataverseHttpHelper.InternalBuildRequestJsonBody(input.EntityData);

        var response = await httpClient.PatchAsync(entitiyUpdateUrl, content, cancellationToken).ConfigureAwait(false);
        var result = await response.InternalReadDataverseResultAsync<TResponseJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(e => new DataverseEntityUpdateOut<TResponseJson>(e));
    }

    private static string BuildEntityUpdateUrl<TRequestJson>(DataverseEntityUpdateIn<TRequestJson> input)
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