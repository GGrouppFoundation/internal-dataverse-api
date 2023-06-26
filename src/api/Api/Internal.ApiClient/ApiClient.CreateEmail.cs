using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    
    private async ValueTask<Result<DataverseEmailCreateOut, Failure<DataverseFailureCode>>> InnerCreateEmailAsync(
        DataverseEmailCreateIn input, CancellationToken cancellationToken = default)
    {
        if (IsInputInvalid(input, out var validationFailure))
        {
            return validationFailure;
        }

        var request = new DataverseHttpRequest<DataverseEmailCreateJsonIn>(
            verb: DataverseHttpVerb.Post,
            url: BuildDataRequestUrl("emails?$select=activityid"),
            headers: new FlatArray<DataverseHttpHeader>(
                new("Accept", "application/json"), 
                new("Prefer", "return=representation")), 
            content: input.MapInput());

        var response = await httpApi
            .InvokeAsync<DataverseEmailCreateJsonIn, DataverseEmailCreateJsonOut>(request, cancellationToken)
            .ConfigureAwait(false);

        return response.MapSuccess(MapSuccess);
    }

    private static DataverseEmailCreateOut MapSuccess(DataverseEmailCreateJsonOut? @out)
        =>
        new(@out?.ActivityId ?? Guid.Empty);

    private static bool IsInputInvalid(DataverseEmailCreateIn input, out Failure<DataverseFailureCode> failure)
    {
        if (input.Sender is null)
        {
            failure = CreateFailure("Input sender is missing");
            return true;
        }
        
        if (string.IsNullOrEmpty(input.Sender.SenderEmail) && input.Sender.SenderMember is null)
        {
            failure = CreateFailure("Input sender is invalid");
            return true;
        }

        if (input.Recipients.IsEmpty)
        {
            failure = CreateFailure("Input recipients are missing");
            return true;
        }

        foreach (var recipient in input.Recipients)
        {
            if (string.IsNullOrEmpty(recipient.SenderRecipientEmail) && recipient.EmailMember is null)
            {
                failure = CreateFailure("Input recipients are invalid");
                return true;
            }
        }

        failure = default;
        return false;

        static Failure<DataverseFailureCode> CreateFailure(string message)
            => 
            Failure.Create(DataverseFailureCode.Unknown, message);
    }
}
