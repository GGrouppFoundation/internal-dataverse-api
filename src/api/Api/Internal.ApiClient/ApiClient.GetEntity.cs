using System;
using System.Collections.Generic;
using System.Net.Http;
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
        using var request = CreateEntityGetRequest(input);

        var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<TJson>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(MapSuccess);

        static DataverseEntityGetOut<TJson> MapSuccess(TJson? @out)
            =>
            new(@out);
    }

    private static HttpRequestMessage CreateEntityGetRequest(DataverseEntityGetIn input)
        =>
        new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = BuildEntityGetUri(input)
        }
        .IncludeAnnotationsHeaderValue(
            input.IncludeAnnotations);

    private static Uri BuildEntityGetUri(DataverseEntityGetIn input)
    {
        var queryParameters = new Dictionary<string, string>
        {
            ["$select"] = QueryParametersBuilder.BuildOdataParameterValue(input.SelectFields)
        };

        var queryString = QueryParametersBuilder.BuildQueryString(queryParameters);

        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);
        return new Uri($"{encodedPluralName}({input.EntityKey.Value}){queryString}", UriKind.Relative);
    }
}