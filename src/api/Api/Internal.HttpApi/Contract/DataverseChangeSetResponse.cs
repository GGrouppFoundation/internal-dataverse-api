using System;

namespace GarageGroup.Infra;

internal readonly record struct DataverseChangeSetResponse
{
    public DataverseChangeSetResponse(FlatArray<DataverseJsonResponse> responses)
        =>
        Responses = responses;

    public FlatArray<DataverseJsonResponse> Responses { get; }
}