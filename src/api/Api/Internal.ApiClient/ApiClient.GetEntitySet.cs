using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GarageGroup.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntitySetGetOut<TJson>, Failure<DataverseFailureCode>>> GetEntitySetAsync<TJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEntitySetGetOut<TJson>>(cancellationToken);
        }

        return InnerGetEntitySetAsync<TJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntitySetGetOut<TJson>, Failure<DataverseFailureCode>>> InnerGetEntitySetAsync<TJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken)
    {
        var request = new DataverseHttpRequest<Unit>(
            verb: DataverseHttpVerb.Get,
            url: BuildEntitySetGetUri(input),
            headers: GetHeaders(),
            content: default);

        var result = await httpApi.InvokeAsync<Unit, DataverseEntitySetJsonGetOut<TJson>>(request, cancellationToken).ConfigureAwait(false);
        return result.MapSuccess(MapSuccess);

        static DataverseEntitySetGetOut<TJson> MapSuccess(DataverseEntitySetJsonGetOut<TJson> success)
            =>
            new(success.Value, success.NextLink);

        FlatArray<DataverseHttpHeader> GetHeaders()
        {
            var preferValue = BuildPreferValue(input.IncludeAnnotations, input.MaxPageSize);

            if (string.IsNullOrEmpty(preferValue))
            {
                return GetAllHeaders();
            }

            return GetAllHeaders(
                new DataverseHttpHeader(PreferHeaderName, preferValue));
        }
    }

    private static string BuildEntitySetGetUri(DataverseEntitySetGetIn input)
    {
        if (string.IsNullOrEmpty(input.NextLink) is false)
        {
            return input.NextLink;
        }

        var queryParameters = new Dictionary<string, string>
        {
            ["$select"] = input.SelectFields.BuildODataParameterValue(),
            ["$expand"] = input.ExpandFields.Map(QueryParametersBuilder.BuildExpandFieldValue).BuildODataParameterValue(),
            ["$filter"] = input.Filter,
            ["$orderby"] = input.OrderBy.Map(GetOrderByValue).BuildODataParameterValue()
        };

        if (input.Top.HasValue)
        {
            queryParameters.Add("$top", input.Top.Value.ToString(CultureInfo.InvariantCulture));
        }

        var queryString = queryParameters.BuildQueryString();

        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);
        return BuildDataRequestUrl(encodedPluralName + queryString);
    }

    private static string GetOrderByValue(DataverseOrderParameter orderParameter)
    {
        if (string.IsNullOrEmpty(orderParameter.FieldName))
        {
            return string.Empty;
        }

        return orderParameter.Direction switch
        {
            DataverseOrderDirection.Ascending => $"{orderParameter.FieldName} asc",
            DataverseOrderDirection.Descending => $"{orderParameter.FieldName} desc",
            _ => orderParameter.FieldName
        };
    }
}