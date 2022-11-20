using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntityCreateOut<TOutJson>, Failure<DataverseFailureCode>>> CreateEntityAsync<TInJson, TOutJson>(
        DataverseEntityCreateIn<TInJson> input, CancellationToken cancellationToken = default)
        where TInJson : notnull
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
        where TInJson : notnull
    {
        using var httpClient = CreateDataHttpClient();
        var entityCreateUrl = BuildEntityCreateUrl(input);

        using var content = input.EntityData.BuildRequestJsonBody();

        var response = await httpClient.PostAsync(entityCreateUrl, content, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<TOutJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(MapSuccess);

        static DataverseEntityCreateOut<TOutJson> MapSuccess(TOutJson? @out)
            =>
            new(@out);
    }

    private static string BuildEntityCreateUrl<TInJson>(DataverseEntityCreateIn<TInJson> input)
        where TInJson : notnull
    {
        var queryParameters = new Dictionary<string, string>
        {
            ["$select"] = input.SelectFields.BuildODataParameterValue()
        };

        var queryString = queryParameters.BuildQueryString();
        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);

        return $"{encodedPluralName}{queryString}";
    }
}