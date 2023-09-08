namespace GarageGroup.Infra;

internal readonly record struct DataverseJsonResponse
{
    public DataverseJsonResponse(DataverseJsonContentOut? content)
        =>
        Content = content;

    public DataverseJsonContentOut? Content { get; }
}