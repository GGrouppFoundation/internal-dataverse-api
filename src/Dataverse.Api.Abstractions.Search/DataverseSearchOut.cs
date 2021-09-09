namespace GGroupp.Infra;

public sealed record DataverseSearchOut
{
    public DataverseSearchOut(
        int totalRecordCount,
        IReadOnlyCollection<DataverseSearchItem> value)
    {
        TotalRecordCount = totalRecordCount;
        Value = value;
    }

    public int TotalRecordCount { get; }

    public IReadOnlyCollection<DataverseSearchItem> Value { get; }
}
