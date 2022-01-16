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

        return cancellationToken.IsCancellationRequested
            ? ValueTask.FromCanceled<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>>(cancellationToken)
            : InnerCreateEntityAsync<TRequestJson, TResponseJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>> InnerCreateEntityAsync<TRequestJson, TResponseJson>(
        DataverseEntityCreateIn<TRequestJson> input, CancellationToken cancellationToken)
    {
        using var httpClient = await DataverseHttpHelper.InternalCreateHttpClientAsync(
                messageHandler,
                configuration,
                apiVersion: ApiVersionData,
                apiType: ApiTypeData)
            .ConfigureAwait(false);

        var entityCreateUrl = BuildEntityCreateUrl(input);

        using var content = DataverseHttpHelper.InternalBuildRequestJsonBody(input.EntityData);

        var response = await httpClient.PostAsync(entityCreateUrl, content, cancellationToken).ConfigureAwait(false);
        var result = await response.InternalReadDataverseResultAsync<TResponseJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(e => new DataverseEntityCreateOut<TResponseJson>(e));
    }

    private static string BuildEntityCreateUrl<TRequestJson>(DataverseEntityCreateIn<TRequestJson> input)
        =>
        Pipeline.Pipe<IReadOnlyCollection<KeyValuePair<string, string>>>(
            new Dictionary<string, string>
            {
                ["$select"] = QueryParametersBuilder.InternalBuildOdataParameterValue(input.SelectFields)
            })
        .Pipe(
            QueryParametersBuilder.InternalBuildQueryString)
        .Pipe(
            queryString => $"{input.EntityPluralName}{queryString}");
}