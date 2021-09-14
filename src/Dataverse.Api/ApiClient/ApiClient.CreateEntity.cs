using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>> CreateEntityAsync<TRequestJson, TResponseJson>(
        DataverseEntityCreateIn<TRequestJson> input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        return cancellationToken.IsCancellationRequested ?
            ValueTask.FromCanceled<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>>(cancellationToken) :
            InternalCreateEntityAsync<TRequestJson, TResponseJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>> InternalCreateEntityAsync<TRequestJson, TResponseJson>(
        DataverseEntityCreateIn<TRequestJson> input, CancellationToken cancellationToken = default)
    {
        var httpClient = await DataverseHttpHelper
            .CreateHttpClientAsync(messageHandler, clientConfiguration, apiType: ApiTypeData)
            .ConfigureAwait(false);

        var entityCreateUrl = BuildEntityCreateUrl(input);

        using var content = DataverseHttpHelper.BuildRequestJsonBody(input.EntityData);

        var response = await httpClient.PostAsync(entityCreateUrl, content, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<TResponseJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(e => new DataverseEntityCreateOut<TResponseJson>(e));
    }

    private static string BuildEntityCreateUrl<TRequestJson>(DataverseEntityCreateIn<TRequestJson> input)
        =>
        Pipeline.Pipe<IReadOnlyCollection<KeyValuePair<string, string>>>(
            new Dictionary<string, string>
            {
                ["$select"] = QueryParametersBuilder.BuildOdataParameterValue(input.SelectFields)
            })
        .Pipe(
            QueryParametersBuilder.BuildQueryString)
        .Pipe(
            queryString => $"{input.EntityPluralName}{queryString}");
}