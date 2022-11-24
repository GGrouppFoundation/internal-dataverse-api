namespace GGroupp.Infra.Dataverse.Api.Test;

internal static partial class ApiClientTestDataSource
{
    private static readonly DataverseHttpHeader PreferRepresentationHeader
        =
        new("Prefer", "return=representation");

    private static DataverseHttpHeader CreateCallerIdHeader(string callerId)
        =>
        new("MSCRMCallerID", callerId);
}