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

        return cancellationToken.IsCancellationRequested ?
            ValueTask.FromCanceled<Result<DataverseEntityUpdateOut<TResponseJson>, Failure<int>>>(cancellationToken) :
            InternalUpdateEntityAsync<TRequestJson, TResponseJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntityUpdateOut<TResponseJson>, Failure<int>>> InternalUpdateEntityAsync<TRequestJson, TResponseJson>(
        DataverseEntityUpdateIn<TRequestJson> input, CancellationToken cancellationToken = default)
    {
        var httpClient = await DataverseHttpHelper
            .CreateHttpClientAsync(messageHandler, clientConfiguration, apiVersion: ApiVersionData, apiType: ApiTypeData)
            .ConfigureAwait(false);

        var entitiyUpdateUrl = BuildEntityUpdateUrl(input);

        using var content = DataverseHttpHelper.BuildRequestJsonBody(input.EntityData);

        var response = await httpClient.PatchAsync(entitiyUpdateUrl, content, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<TResponseJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(e => new DataverseEntityUpdateOut<TResponseJson>(e));
    }

    private static string BuildEntityUpdateUrl<TRequestJson>(DataverseEntityUpdateIn<TRequestJson> input)
        =>
        Pipeline.Pipe<IReadOnlyCollection<KeyValuePair<string, string>>>(
            new Dictionary<string, string>
            {
                ["$select"] = QueryParametersBuilder.BuildOdataParameterValue(input.SelectFields)
            })
        .Pipe(
            QueryParametersBuilder.BuildQueryString)
        .Pipe(
            queryString => $"{input.EntityPluralName}({input.EntityKey.Value}){queryString}");
}