using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GarageGroup.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<Unit, Failure<DataverseFailureCode>>> UpdateEntityAsync<TInJson>(
        DataverseEntityUpdateIn<TInJson> input, CancellationToken cancellationToken = default)
        where TInJson : notnull
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<Unit>(cancellationToken);
        }

        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);

        var request = new DataverseHttpRequest<TInJson>(
            verb: DataverseHttpVerb.Patch,
            url: BuildDataRequestUrl($"{encodedPluralName}({input.EntityKey.Value})"),
            headers: GetAllHeadersWithoutRepresentation(input.SuppressDuplicateDetection),
            content: new(input.EntityData));

        return httpApi.InvokeAsync<TInJson, Unit>(request, cancellationToken);
    }
}