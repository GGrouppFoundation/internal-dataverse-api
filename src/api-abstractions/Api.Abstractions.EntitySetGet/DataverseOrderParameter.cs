namespace GarageGroup.Infra;

public sealed record class DataverseOrderParameter
{
    public DataverseOrderParameter(string fieldName, DataverseOrderDirection direction)
    {
        FieldName = fieldName ?? string.Empty;
        Direction = direction;
    }

    public DataverseOrderParameter(string fieldName)
    {
        FieldName = fieldName ?? string.Empty;
        Direction = DataverseOrderDirection.Default;
    }

    public string FieldName { get; }

    public DataverseOrderDirection Direction { get; }
}