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
        using var httpClient = CreateDataHttpClient();
        var entityCreateUrl = BuildEntityCreateUrl(input);

        using var content = DataverseHttpHelper.BuildRequestJsonBody(input.EntityData);

        var response = await httpClient.PostAsync(entityCreateUrl, content, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<TOutJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(e => new DataverseEntityCreateOut<TOutJson>(e));
    }

    private static string BuildEntityCreateUrl<TInJson>(DataverseEntityCreateIn<TInJson> input)
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