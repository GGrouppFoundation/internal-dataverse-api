using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>> CreateEntityAsync<TRequestJson, TResponseJson>(
        DataverseEntityCreateIn<TRequestJson> input, CancellationToken cancellationToken = default)
        =>
        (input, cancellationToken.IsCancellationRequested) switch
        {
            (null, _) => throw new ArgumentNullException(nameof(input)),
            (_, true) => ValueTask.FromCanceled<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>>(cancellationToken),
            _ => InternalCreateEntityAsync<TRequestJson, TResponseJson>(input, cancellationToken)
        };

    private async ValueTask<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>> InternalCreateEntityAsync<TRequestJson, TResponseJson>(
        DataverseEntityCreateIn<TRequestJson> input, CancellationToken cancellationToken = default)
    {
        var httpClient = await DataverseHttpHelper.CreateHttpClientAsync(messageHandler, clientConfiguration);

        var entitiyCreateUrl = BuildEntityCreateUrl(input);

        using var content = DataverseHttpHelper.BuildRequestJsonBody(input.EntityData);

        var response = await httpClient.PostAsync(entitiyCreateUrl, content, cancellationToken).ConfigureAwait(false);
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