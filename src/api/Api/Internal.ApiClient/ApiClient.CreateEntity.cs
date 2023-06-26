using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GarageGroup.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<Unit, Failure<DataverseFailureCode>>> CreateEntityAsync<TInJson>(
        DataverseEntityCreateIn<TInJson> input, CancellationToken cancellationToken = default)
        where TInJson : notnull
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<Unit>(cancellationToken);
        }

        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);

        var request = new DataverseHttpRequest<TInJson>(
            verb: DataverseHttpVerb.Post,
            url: BuildDataRequestUrl(encodedPluralName),
            headers: GetAllHeadersWithoutRepresentation(input.SuppressDuplicateDetection),
            content: new(input.EntityData));

        return httpApi.InvokeAsync<TInJson, Unit>(request, cancellationToken);
    }
}