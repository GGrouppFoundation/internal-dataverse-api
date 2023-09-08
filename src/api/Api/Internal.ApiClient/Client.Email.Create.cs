using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal sealed partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEmailCreateOut, Failure<DataverseFailureCode>>> CreateEmailAsync(
        DataverseEmailCreateIn input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEmailCreateOut>(cancellationToken);
        }

        return InnerCreateEmailAsync(input, cancellationToken);
    }

    private ValueTask<Result<DataverseEmailCreateOut, Failure<DataverseFailureCode>>> InnerCreateEmailAsync(
        DataverseEmailCreateIn input, CancellationToken cancellationToken)
        =>
        GetJsonOrFailure(input).ForwardValueAsync(InnerCreateEmailAsync, cancellationToken);

    private async ValueTask<Result<DataverseEmailCreateOut, Failure<DataverseFailureCode>>> InnerCreateEmailAsync(
        DataverseEmailCreateJsonIn input, CancellationToken cancellationToken)
    {
        try
        {
            var request = new DataverseJsonRequest(
                verb: DataverseHttpVerb.Post,
                url: BuildDataRequestUrl("emails?$select=activityid"),
                headers: new FlatArray<DataverseHttpHeader>(
                    new("Accept", "application/json"), 
                    new("Prefer", "return=representation")), 
                content: input.SerializeOrThrow());

            var result = await httpApi.SendJsonAsync(request, cancellationToken).ConfigureAwait(false);
            return result.MapSuccess(InnerMapSuccess);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return ToDataverseFailure(ex, "An unexpected exception was thrown when trying to create a Dataverse email");
        }

        static DataverseEmailCreateOut InnerMapSuccess(DataverseJsonResponse response)
            =>
            new(
                emailId: response.Content.DeserializeOrThrow<DataverseEmailCreateJsonOut>()?.ActivityId ?? default);
    }

    private static Result<DataverseEmailCreateJsonIn, Failure<DataverseFailureCode>> GetJsonOrFailure(DataverseEmailCreateIn input)
    {
        if (input.Sender is null)
        {
            return CreateFailure("Input sender is missing");
        }
        
        if (string.IsNullOrEmpty(input.Sender.SenderEmail) && input.Sender.SenderMember is null)
        {
            return CreateFailure("Input sender is invalid");
        }

        if (input.Recipients.IsEmpty)
        {
            return CreateFailure("Input recipients are missing");
        }

        foreach (var recipient in input.Recipients)
        {
            if (string.IsNullOrEmpty(recipient.SenderRecipientEmail) && recipient.EmailMember is null)
            {
                return CreateFailure("Input recipients are invalid");
            }
        }

        return input.MapInput();

        static Failure<DataverseFailureCode> CreateFailure(string message)
            => 
            Failure.Create(DataverseFailureCode.Unknown, message);
    }
}
