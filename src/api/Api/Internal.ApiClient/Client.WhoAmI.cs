using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseWhoAmIOut, Failure<DataverseFailureCode>>> WhoAmIAsync(
        Unit input, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseWhoAmIOut>(cancellationToken);
        }

        return InnerWhoAmIAsync(cancellationToken);
    }

    private async ValueTask<Result<DataverseWhoAmIOut, Failure<DataverseFailureCode>>> InnerWhoAmIAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new DataverseJsonRequest(
                verb: DataverseHttpVerb.Get,
                url: BuildDataRequestUrl(WhoAmIRelativeUrl),
                headers: GetAllHeaders(),
                content: default);

            var result = await httpApi.SendJsonAsync(request, cancellationToken).ConfigureAwait(false);
            return result.MapSuccess(MapSuccess);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return ToDataverseFailure(ex, "An unexpected exception was thrown when trying to get a Dataverse current user data");
        }

        static DataverseWhoAmIOut MapSuccess(DataverseJsonResponse response)
        {
            var json = response.Content.DeserializeOrThrow<DataverseWhoAmIOutJson>();

            return new(
                businessUnitId: json.BusinessUnitId,
                userId: json.UserId,
                organizationId: json.OrganizationId);
        }
    }
}