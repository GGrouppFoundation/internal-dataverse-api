using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal sealed partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEmailSendOut, Failure<DataverseFailureCode>>> SendEmailAsync(
        DataverseEmailSendIn input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEmailSendOut>(cancellationToken);
        }

        return InnerSendEmailAsync(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEmailSendOut, Failure<DataverseFailureCode>>> InnerSendEmailAsync(
        DataverseEmailSendIn input, CancellationToken cancellationToken)
    {
        try
        {
            if (input.EmailId is not null)
            {
                return await InnerSendEmailAsync(input.EmailId.Value, cancellationToken).ConfigureAwait(false);
            }

            var creationResult = await InnerCreateEmailAsync(input, cancellationToken).ConfigureAwait(false);
            return await creationResult.ForwardValueAsync(InnerSendEmailAsync, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return ToDataverseFailure(ex, "An unexpected exception was thrown when trying to send a Dataverse email");
        }
    }

    private async ValueTask<Result<Guid, Failure<DataverseFailureCode>>> InnerCreateEmailAsync(
        DataverseEmailSendIn input, CancellationToken cancellationToken)
    {
        var @in = new DataverseEmailCreateIn(
            subject: input.Subject,
            body: input.Body,
            sender: input.Sender,
            recipients: input.Recipients,
            extensionData: input.ExtensionData);

        var result = await InnerCreateEmailAsync(@in, cancellationToken).ConfigureAwait(false);
        return result.MapSuccess(GetEmailId);

        static Guid GetEmailId(DataverseEmailCreateOut @out)
            =>
            @out.EmailId;
    }

    private async ValueTask<Result<DataverseEmailSendOut, Failure<DataverseFailureCode>>> InnerSendEmailAsync(
        Guid emailId, CancellationToken cancellationToken)
    {
        var emailSendJsonIn = new DataverseEmailSendJsonIn
        {
            IssueSend = true
        };

        var request = new DataverseJsonRequest(
            verb: DataverseHttpVerb.Post,
            url: BuildDataRequestUrl($"emails({emailId:D})/Microsoft.Dynamics.CRM.SendEmail"),
            headers: GetAllHeaders(),
            content: emailSendJsonIn.SerializeOrThrow());

        var result = await httpApi.SendJsonAsync(request, cancellationToken).ConfigureAwait(false);
        return result.MapSuccess(MapSuccess);

        DataverseEmailSendOut MapSuccess(DataverseJsonResponse _)
            =>
            new(emailId);
    }
}