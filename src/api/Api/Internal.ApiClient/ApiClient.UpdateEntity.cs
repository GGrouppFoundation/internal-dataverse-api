using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntityUpdateOut<TOutJson>, Failure<DataverseFailureCode>>> UpdateEntityAsync<TInJson, TOutJson>(
        DataverseEntityUpdateIn<TInJson> input, CancellationToken cancellationToken = default)
        where TInJson : notnull
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
        where TInJson : notnull
    {
        using var httpClient = CreateDataHttpClient();
        var entitiyUpdateUrl = BuildEntityUpdateUrl(input);

        using var content = DataverseHttpHelper.BuildRequestJsonBody(input.EntityData);

        var response = await httpClient.PatchAsync(entitiyUpdateUrl, content, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<TOutJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(MapSuccess);

        static DataverseEntityUpdateOut<TOutJson> MapSuccess(TOutJson? @out)
            =>
            new(@out);
    }

    private static string BuildEntityUpdateUrl<TInJson>(DataverseEntityUpdateIn<TInJson> input)
        where TInJson : notnull
    {
        var queryParameters = new Dictionary<string, string>
        {
            ["$select"] = QueryParametersBuilder.BuildOdataParameterValue(input.SelectFields)
        };

        var queryString = QueryParametersBuilder.BuildQueryString(queryParameters);

        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);
        return $"{encodedPluralName}({input.EntityKey.Value}){queryString}";
    }
}