using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntitySetGetOut<TJson>, Failure<DataverseFailureCode>>> GetEntitySetAsync<TJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEntitySetGetOut<TJson>>(cancellationToken);
        }

        return InnerGetEntitySetAsync<TJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntitySetGetOut<TJson>, Failure<DataverseFailureCode>>> InnerGetEntitySetAsync<TJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken)
    {
        using var httpClient = CreateDataHttpClient();
        using var request = CreateEntitySetGetRequest(input);

        var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<DataverseEntitySetJsonGetOut<TJson>>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(success => new DataverseEntitySetGetOut<TJson>(success?.Value));
    }

    private static HttpRequestMessage CreateEntitySetGetRequest(DataverseEntitySetGetIn input)
        =>
        new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = BuildEntitySetGetUri(input)
        }
        .IncludeAnnotationsHeaderValue(
            input.IncludeAnnotations);

    private static Uri BuildEntitySetGetUri(DataverseEntitySetGetIn input)
    {
        var queryParameters = new Dictionary<string, string>
        {
            ["$select"] = QueryParametersBuilder.BuildOdataParameterValue(input.SelectFields),
            ["$filter"] = input.Filter,
            ["$orderby"] = string.Join(',', input.OrderBy.Where(NotEmptyFieldName).Select(GetOrderByValue))
        };

        if (input.Top.HasValue)
        {
            queryParameters.Add("$top", input.Top.Value.ToString(CultureInfo.InvariantCulture));
        }

        var queryString = QueryParametersBuilder.BuildQueryString(queryParameters);

        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);
        return new Uri(encodedPluralName + queryString, UriKind.Relative);
    }

    private static bool NotEmptyFieldName(DataverseOrderParameter orderParameter)
        =>
        string.IsNullOrEmpty(orderParameter.FieldName) is false;

    private static string GetOrderByValue(DataverseOrderParameter orderParameter)
        =>
        orderParameter.Direction switch
        {
            DataverseOrderDirection.Ascending => $"{orderParameter.FieldName} asc",
            DataverseOrderDirection.Descending => $"{orderParameter.FieldName} desc",
            _ => orderParameter.FieldName
        };
}