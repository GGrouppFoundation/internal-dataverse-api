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
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEntityGetOut<TJson>>(cancellationToken);
        }

        return InnerGetEntityAsync<TJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntityGetOut<TJson>, Failure<DataverseFailureCode>>> InnerGetEntityAsync<TJson>(
        DataverseEntityGetIn input, CancellationToken cancellationToken)
    {
        var queryParameters = new Dictionary<string, string>
        {
            ["$select"] = input.SelectFields.BuildODataParameterValue(),
            ["$expand"] = input.ExpandFields.Map(QueryParametersBuilder.BuildExpandFieldValue).BuildODataParameterValue()
        };

        var queryString = queryParameters.BuildQueryString();
        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);

        var request = new DataverseHttpRequest<Unit>(
            verb: DataverseHttpVerb.Get,
            url: BuildDataRequestUrl($"{encodedPluralName}({input.EntityKey.Value}){queryString}"),
            headers: GetHeaders(),
            content: default);

        var result = await httpApi.InvokeAsync<Unit, TJson>(request, cancellationToken).ConfigureAwait(false);
        return result.MapSuccess(MapSuccess);

        static DataverseEntityGetOut<TJson> MapSuccess(TJson? @out)
            =>
            new(@out);

        FlatArray<DataverseHttpHeader> GetHeaders()
        {
            var preferValue = BuildPreferValue(input.IncludeAnnotations);

            if (string.IsNullOrEmpty(preferValue))
            {
                return GetAllHeaders();
            }

            return GetAllHeaders(
                new DataverseHttpHeader(PreferHeaderName, preferValue));
        }
    }
}