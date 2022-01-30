using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntityCreateOut<TOutJson>, Failure<DataverseFailureCode>>> CreateEntityAsync<TInJson, TOutJson>(
        DataverseEntityCreateIn<TInJson> input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEntityCreateOut<TOutJson>>(cancellationToken);
        }

        return InnerCreateEntityAsync<TInJson, TOutJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntityCreateOut<TOutJson>, Failure<DataverseFailureCode>>> InnerCreateEntityAsync<TInJson, TOutJson>(
        DataverseEntityCreateIn<TInJson> input, CancellationToken cancellationToken)
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
        var result = await response.InternalReadDataverseResultAsync<TOutJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(e => new DataverseEntityCreateOut<TOutJson>(e));
    }

    private static string BuildEntityCreateUrl<TInJson>(DataverseEntityCreateIn<TInJson> input)
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