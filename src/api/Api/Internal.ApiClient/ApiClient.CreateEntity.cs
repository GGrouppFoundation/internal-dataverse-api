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
        ArgumentNullException.ThrowIfNull(input);

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
        var queryParameters = new Dictionary<string, string>
        {
            ["$select"] = input.SelectFields.BuildODataParameterValue()
        };

        var queryString = queryParameters.BuildQueryString();
        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);

        var request = new DataverseHttpRequest<TInJson>(
            verb: DataverseHttpVerb.Post,
            url: BuildDataRequestUrl($"{encodedPluralName}{queryString}"),
            headers: GetAllHeaders(
                new DataverseHttpHeader(PreferHeaderName, ReturnRepresentationValue)),
            content: new(input.EntityData));

        var result = await httpApi.InvokeAsync<TInJson, TOutJson>(request, cancellationToken).ConfigureAwait(false);
        return result.MapSuccess(MapSuccess);

        static DataverseEntityCreateOut<TOutJson> MapSuccess(TOutJson? success)
            =>
            new(success);
    }
}