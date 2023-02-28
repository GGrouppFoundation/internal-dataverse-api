using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal sealed partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEmailSendOut, Failure<DataverseFailureCode>>> SendEmailAsync(
        DataverseEmailSendIn input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEmailSendOut>(cancellationToken);
        }

        return InnerSendEmailAsync(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEmailSendOut, Failure<DataverseFailureCode>>> InnerSendEmailAsync(
        DataverseEmailSendIn input,
        CancellationToken cancellationToken = default)
    {
        if (input.EmailId.HasValue && input.EmailId.Value != Guid.Empty)
        {
            var result = await httpApi.InvokeAsync<DataverseEmailSendJsonIn, Unit>(CreateEmailSendRequest(input.EmailId.Value), cancellationToken);
            return result.MapSuccess(MapSuccessSendEmail);
        }

        var creationResult = await CreateEmailAsync(MapInputForCreation(input), cancellationToken).ConfigureAwait(false);
        if (creationResult.IsFailure)
        {
            return creationResult.FailureOrThrow();
        }

        var emailId = creationResult.SuccessOrThrow().EmailId;
        
        var sendResult = await httpApi
            .InvokeAsync<DataverseEmailSendJsonIn, Unit>(CreateEmailSendRequest(emailId), cancellationToken)
            .ConfigureAwait(false);

        return sendResult.MapSuccess(MapSuccessCreateSendEmail);

        DataverseEmailSendOut MapSuccessSendEmail(Unit _) => new(input.EmailId.Value);
        DataverseEmailSendOut MapSuccessCreateSendEmail(Unit _) => new(emailId);
    }

    private static DataverseEmailCreateIn MapInputForCreation(DataverseEmailSendIn input)
        =>
        new(
            subject: input.Subject,
            body: input.Body,
            sender: input.Sender,
            recipients: input.Recipients,
            extensionData: input.ExtensionData);

    private static DataverseHttpRequest<DataverseEmailSendJsonIn> CreateEmailSendRequest(Guid? emailId)
        =>
        new(
            verb: DataverseHttpVerb.Post,
            url: BuildDataRequestUrl($"emails({emailId:D})/Microsoft.Dynamics.CRM.SendEmail"),
            headers: FlatArray<DataverseHttpHeader>.Empty,
            content: new DataverseEmailSendJsonIn { IssueSend = true });
}