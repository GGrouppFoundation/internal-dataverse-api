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
        ArgumentNullException.ThrowIfNull(input);

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
        var queryParameters = new Dictionary<string, string>
        {
            ["$select"] = input.SelectFields.BuildODataParameterValue()
        };

        var queryString = queryParameters.BuildQueryString();
        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);

        var request = new DataverseHttpRequest<TInJson>(
            verb: DataverseHttpVerb.Patch,
            url: BuildDataRequestUrl($"{encodedPluralName}({input.EntityKey.Value}){queryString}"),
            headers: GetAllHeaders(
                new DataverseHttpHeader(PreferHeaderName, ReturnRepresentationValue)),
            content: new(input.EntityData));

        var result = await httpApi.InvokeAsync<TInJson, TOutJson>(request, cancellationToken).ConfigureAwait(false);
        return result.MapSuccess(MapSuccess);

        static DataverseEntityUpdateOut<TOutJson> MapSuccess(TOutJson? @out)
            =>
            new(@out);
    }
}