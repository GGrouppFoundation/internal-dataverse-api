using System;

namespace GarageGroup.Infra;

internal sealed record class DataverseChangeSetRequest
{
    public DataverseChangeSetRequest(
        string? url,
        Guid batchId,
        Guid changeSetId,
        FlatArray<DataverseHttpHeader> headers,
        FlatArray<DataverseJsonRequest> requests)
    {
        Url = url;
        BatchId = batchId;
        ChangeSetId = changeSetId;
        Headers = headers;
        Requests = requests;
    }

    public string? Url { get; }

    public Guid BatchId { get; }

    public Guid ChangeSetId { get; }

    public FlatArray<DataverseHttpHeader> Headers { get; }

    public FlatArray<DataverseJsonRequest> Requests { get; }
}
