using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GarageGroup.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<Unit, Failure<DataverseFailureCode>>> DeleteEntityAsync(
        DataverseEntityDeleteIn input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<Unit>(cancellationToken);
        }

        var encodedPluralName = HttpUtility.UrlEncode(input.EntityPluralName);

        var request = new DataverseHttpRequest<Unit>(
            verb: DataverseHttpVerb.Delete,
            url: BuildDataRequestUrl($"{encodedPluralName}({input.EntityKey.Value})"),
            headers: GetAllHeaders(),
            content: default);
        
        return httpApi.InvokeAsync<Unit, Unit>(request, cancellationToken);
    }
}