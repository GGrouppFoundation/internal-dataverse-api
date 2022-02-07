using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntityGetOut<TJson>, Failure<DataverseFailureCode>>> GetEntityAsync<TJson>(
        DataverseEntityGetIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEntityGetOut<TJson>>(cancellationToken);
        }

        return InnerGetEntityAsync<TJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntityGetOut<TJson>, Failure<DataverseFailureCode>>> InnerGetEntityAsync<TJson>(
        DataverseEntityGetIn input, CancellationToken cancellationToken)
    {
        using var httpClient = CreateDataHttpClient();
        var entitiesGetUrl = BuildEntityGetUrl(input);

        var response = await httpClient.GetAsync(entitiesGetUrl, cancellationToken).ConfigureAwait(false);
        var result = await response.InternalReadDataverseResultAsync<TJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(e => new DataverseEntityGetOut<TJson>(e));
    }

    private static string BuildEntityGetUrl(DataverseEntityGetIn input)
    {
        var queryParameters = new Dictionary<string, string>
        {
            ["$select"] = QueryParametersBuilder.InternalBuildOdataParameterValue(input.SelectFields)
        };

        var queryString = QueryParametersBuilder.InternalBuildQueryString(queryParameters);

        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);
        var encodedKey = HttpUtility.UrlEncode(input.EntityKey.Value);

        return $"{encodedPluralName}({encodedKey}){queryString}";
    }
}